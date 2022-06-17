using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;

public partial class PHBaseNPC : AnimatedEntity
{
	public virtual string NPCName => "Base NPC";
	public virtual string ModelPath => "models/citizen/citizen.vmdl";

	DamageInfo lastDMG;

	[ConVar.Replicated]
	public static bool ph_nav_drawpath { get; set; }

	[ConCmd.Admin( "ph_npc_clear" )]
	public static void ClearAllNPCs()
	{
		foreach ( var npc in All.OfType<PHBaseNPC>().ToArray() )
			npc.Delete();
	}

	//NPCPath Path = new NPCPath();
	//public NPCSteer Steering;

	public override void Spawn()
	{
		base.Spawn();

		SetModel( ModelPath );

		EyePosition = Position + Vector3.Up * 64;
		CollisionGroup = CollisionGroup.Player;
		SetupPhysicsFromCapsule( PhysicsMotionType.Keyframed, Capsule.FromHeightAndRadius( 72, 8 ) );

		EnableHitboxes = true;
		EnableLagCompensation = true;

		//Steering = new NPCWander();

		SetBodyGroup( 1, 0 );
	}

	public NPCDebugDraw Draw => NPCDebugDraw.Once;

	//Vector3 InputVelocity;

	//Vector3 LookDir;

	[Event.Tick.Server]
	public void Tick()
	{
		//using var _a = Profile.Scope( "NpcTest::Tick" );
		/*
		InputVelocity = 0;

		if ( Steering != null )
		{
			using var _b = Profile.Scope( "Steer" );

			Steering.Tick( Position );

			if ( !Steering.Output.Finished )
			{
				InputVelocity = Steering.Output.Direction.Normal;
				Velocity = Velocity.AddClamped( InputVelocity * Time.Delta * 500, 50.0f );
			}

			if ( pd_nav_drawpath )
			{
				Steering.DebugDrawPath();
			}
		}

		using ( Profile.Scope( "Move" ) )
		{
			Move( Time.Delta );
		}

		var walkVelocity = Velocity.WithZ( 0 );
		if ( walkVelocity.Length > 0.5f )
		{
			var turnSpeed = walkVelocity.Length.LerpInverse( 0, 100, true );
			var targetRotation = Rotation.LookAt( walkVelocity.Normal, Vector3.Up );
			Rotation = Rotation.Lerp( Rotation, targetRotation, turnSpeed * Time.Delta * 20.0f );
		}*/

		/*var animHelper = new NPCHelper( this );

		LookDir = Vector3.Lerp( LookDir, InputVelocity.WithZ( 0 ) * 1000, Time.Delta * 100.0f );
		animHelper.WithLookAt( EyePosition + LookDir );
		animHelper.WithVelocity( Velocity );
		animHelper.WithWishVelocity( InputVelocity );*/
	}

	public override void TakeDamage( DamageInfo info )
	{
		if ( info.Attacker is PHPawn )
			return;

		lastDMG = info;
		base.TakeDamage( info );
	}

	public virtual void InteractWith(PHPawn player)
	{
	}

	protected virtual void Move( float timeDelta )
	{
		var bbox = BBox.FromHeightAndRadius( 64, 4 );

		MoveHelper move = new( Position, Velocity );
		move.MaxStandableAngle = 50;
		move.Trace = move.Trace.Ignore( this ).Size( bbox );

		if ( !Velocity.IsNearlyZero( 0.001f ) )
		{
			using ( Profile.Scope( "TryUnstuck" ) )
				move.TryUnstuck();

			using ( Profile.Scope( "TryMoveWithStep" ) )
				move.TryMoveWithStep( timeDelta, 30 );
		}

		using ( Profile.Scope( "Ground Checks" ) )
		{
			var tr = move.TraceDirection( Vector3.Down * 10.0f );

			if ( move.IsFloor( tr ) )
			{
				GroundEntity = tr.Entity;

				if ( !tr.StartedSolid )
				{
					move.Position = tr.EndPosition;
				}

				/*if ( InputVelocity.Length > 0 )
				{
					var movement = move.Velocity.Dot( InputVelocity.Normal );
					move.Velocity = move.Velocity - movement * InputVelocity.Normal;
					move.ApplyFriction( tr.Surface.Friction * 10.0f, timeDelta );
					move.Velocity += movement * InputVelocity.Normal;

				}*/
				//else
				//{
					move.ApplyFriction( tr.Surface.Friction * 10.0f, timeDelta );
				//}
			}
			else
			{
				GroundEntity = null;
				move.Velocity += Vector3.Down * 900 * timeDelta;
				NPCDebugDraw.Once.WithColor( Color.Red ).Circle( Position, Vector3.Up, 10.0f );
			}
		}

		Position = move.Position;
		Velocity = move.Velocity;
	}

	public override void OnKilled()
	{
		base.OnKilled();

		CreateNPCRagdoll( lastDMG.Force, lastDMG.HitboxIndex );
	}

	public virtual void CreateNPCRagdoll( Vector3 force, int forceBone )
	{
		var ent = new ModelEntity();
		ent.Position = Position;
		ent.Rotation = Rotation;
		ent.MoveType = MoveType.Physics;
		ent.UsePhysicsCollision = true;
		ent.SetInteractsAs( CollisionLayer.Debris );
		ent.SetInteractsWith( CollisionLayer.WORLD_GEOMETRY );
		ent.SetInteractsExclude( CollisionLayer.Player | CollisionLayer.Debris );

		ent.CopyFrom( this );
		ent.CopyBonesFrom( this );
		ent.SetRagdollVelocityFrom( this );
		ent.DeleteAsync( 20.0f );

		// Copy the clothes over
		foreach ( var child in Children )
		{
			if ( !child.Tags.Has( "clothes" ) )
				continue;

			if ( child is ModelEntity e )
			{
				var clothing = new ModelEntity();
				clothing.CopyFrom( e );
				clothing.SetParent( ent, true );
			}
		}

		ent.PhysicsGroup.AddVelocity( force );

		if ( forceBone >= 0 )
		{
			var body = ent.GetBonePhysicsBody( forceBone );
			if ( body != null )
			{
				body.ApplyForce( force * 1000 );
			}
			else
			{
				ent.PhysicsGroup.AddVelocity( force );
			}
		}

		ent.DeleteAsync( 20.0f );
	}
}
