using System.Collections.Generic;
using System.Linq;
using Sandbox;


//Base of PlayHome's player pawns
public partial class BasePawn : Player
{
	[Net, Predicted]
	public IList<Entity> ActiveChildren { get; set; }

	public List<AchBase> AchList;

	public PHInventorySystem PHInventory;

	public List<AchBase> AchChecker;

	DamageInfo lastDMGInfo;

	bool UpdateViewAngle;
	Angles UpdatedViewAngle;

	public void SetUpPlayer()
	{
		SetModel( "models/citizen/citizen.vmdl" );

		//Temporary, should have a custom camera
		CameraMode = new FirstPersonCamera();

		Animator = new StandardPlayerAnimator();

		//Should we have our own walk controller?
		Controller = new WalkController();

		EnableDrawing = true;
		EnableHideInFirstPerson = true;
		EnableShadowInFirstPerson = true;

		if ( AchList == null )
			AchList = new List<AchBase>();

		if ( AchChecker == null )
			AchChecker = new List<AchBase>();
	}

	public override void Spawn()
	{
		base.Spawn();

		SetUpPlayer();

		//Use the base player respawn, NOT the respawn in this class
		base.Respawn();

		SetInteractsAs( CollisionLayer.Player );
		SetInteractsExclude( CollisionLayer.Player );
	}

	public override void Respawn()
	{
		base.Respawn();

		SetInteractsAs( CollisionLayer.Player );
		SetInteractsExclude( CollisionLayer.Player );

		SetUpPlayer();

		//Deletes the corpse if valid
		DestroyCorpse(To.Everyone);
		
		//this is temporary, just to show off glasses to admins/devs
		if( PHGame.Instance.AdminList.Contains(Client.PlayerId ) )
		{
			var adminGlasses = new ModelEntity();
			adminGlasses.SetModel( "models/cloth/dealwithitglass/dwi_glass.vmdl" );
			adminGlasses.SetParent( this, true );
			
			adminGlasses.EnableHideInFirstPerson = true;
		}

		if ( ActiveChildren == null )
			ActiveChildren = new List<Entity>();
	}

	//Thanks Crayz
	[ClientRpc]
	public void SetViewAngles( Angles angles )
	{
		UpdateViewAngle = true;
		UpdatedViewAngle = angles;
	}

	public override void BuildInput( InputBuilder input )
	{
		base.BuildInput( input );

		if ( UpdateViewAngle )
		{
			UpdateViewAngle = false;
			input.ViewAngles = UpdatedViewAngle;
		}
	}

	//Simulation on both server and client
	public override void Simulate( Client cl )
	{
		//We won't call the base simulate since it automatically respawns the player on death in a few seconds
		//but use the last 2 lines of it

		var controller = GetActiveController();
		controller?.Simulate( cl, this, GetActiveAnimator() );

		if ( cl.GetClientData( "cl_showfps", false ) )
		{
			var fps = 1 / RealTime.Delta;
			DebugOverlay.ScreenText( $"{fps.ToString( "0.00" )} FPS", -2 );
		}
	}

	public override void TakeDamage( DamageInfo info )
	{
		if ( info.Attacker is LobbyPawn )
			return;

		lastDMGInfo = info;
		base.TakeDamage( info );
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
	}

	//Frame simulated on the client
	public override void FrameSimulate( Client cl )
	{
		base.FrameSimulate( cl );
	}

	public virtual void CheckOrUpdateAchievement(string achievement, string className)
	{
		if ( AchList.FirstOrDefault( x => x.AchName == achievement ) == null )
		{
			if ( AchChecker.FirstOrDefault( x => x.AchName == achievement ) == null )
				AchChecker.Add( TypeLibrary.Create<AchBase>( className ) );
		}

		var achUpdate = AchChecker.FirstOrDefault( x => x.AchName == achievement ) ?? null;

		if ( achUpdate != null && !achUpdate.HasCompleted )
		{
			AchChecker.First( x => x.AchName == achievement ).UpdateAchievement( this );
		}
	}

	public override void OnAnimEventFootstep( Vector3 pos, int foot, float volume )
	{
		if(IsServer)
		{
			CheckOrUpdateAchievement( "Walk Marathon", "WalkMarathon" );
		}

		base.OnAnimEventFootstep( pos, foot, volume * 10 );
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
