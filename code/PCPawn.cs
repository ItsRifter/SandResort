using Sandbox;
using System;
using System.Linq;


public partial class PCPawn : Player
{

	DamageInfo lastDMGInfo;

	//Don't allow players to spam death and respawning
	TimeSince timeLastDied;
	public TimeSince timeLastRespawn;

	public ModelEntity playerCorpse { get; set; }

	public override void Spawn()
	{
		base.Spawn();

		SetModel( "models/citizen/citizen.vmdl" );

		//Temporary, should have a custom camera
		CameraMode = new FirstPersonCamera();

		Animator = new StandardPlayerAnimator();

		//Should we have our own walk controller?
		Controller = new WalkController();

		EnableDrawing = true;
		EnableHideInFirstPerson = true;
		EnableShadowInFirstPerson = true;

		//Use the base player respawn, NOT the respawn in this class
		base.Respawn();
	}

	public override void Respawn()
	{
		base.Respawn();
		CameraMode = new FirstPersonCamera();

		EnableAllCollisions = true;
		EnableDrawing = true;

		//Deletes the corpse if valid
		DestroyCorpse();
		

		timeLastRespawn = 0;
	}

	//Simulation on both server and client
	public override void Simulate( Client cl )
	{
		//We won't call the base simulate since it automatically respawns the player on death in a few seconds
		//but use the last 2 lines of it

		var controller = GetActiveController();
		controller?.Simulate( cl, this, GetActiveAnimator() );

		if ( IsServer )
		{
			if ( LifeState == LifeState.Alive )
				SimulateActions();
			else if ( LifeState == LifeState.Dead )
				SimulateActionsWhilstDead();
		}
	}

	public void SimulateActions()
	{

	}

	public void SimulateActionsWhilstDead()
	{
		if(Input.Pressed(InputButton.PrimaryAttack) && timeLastDied > 3.0f)
			Respawn();
	}

	//When the player is killed
	public override void OnKilled()
	{
		base.OnKilled();

		EnableAllCollisions = false;
		EnableDrawing = false;

		CreatePlayerRagdoll( lastDMGInfo.Force, lastDMGInfo.BoneIndex );

		//We should make a first person death camera in the future
		CameraMode = new RagdollCamera();

		timeLastDied = 0;
	}

	//Frame simulated on the client
	public override void FrameSimulate( Client cl )
	{
		base.FrameSimulate( cl );
	}

	//Creates a player ragdoll with clothing (if any) at force with bone index
	[ClientRpc]
	public void CreatePlayerRagdoll( Vector3 force, int forceBone )
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

		Corpse = ent;
	}

	[ClientRpc]
	public void DestroyCorpse()
	{
		if ( !Corpse.IsValid() )
			return;

		Corpse.Delete();
		Corpse = null;
	}
}
