using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;


//Base of PlayHome's player pawns
public partial class BasePawn : Player, IPlayerData
{
	public AchTracker AchTracker { get; protected set; }

	public string PlayerName => Client.Name;

	public IList<AchBase> Achievements => GetAchievements();

	DamageInfo lastDMGInfo;

	bool updateViewAngle;
	Angles updatedViewAngle;


	public BasePawn()
	{
		AchTracker = new AchTracker();
	}

	public void GoToSpawnpoint()
	{
		var spawnpoint = All.OfType<SpawnPoint>().OrderBy( x => Guid.NewGuid() ).FirstOrDefault();

		if ( spawnpoint != null )
		{
			SetViewAngles( spawnpoint.Rotation.Angles() );
			Transform = spawnpoint.Transform;
		}
	}

	public void SetUpPlayer()
	{
		Host.AssertServer();

		LifeState = LifeState.Alive;
		Health = 100;
		Velocity = Vector3.Zero;
		WaterLevel = 0;

		GoToSpawnpoint();
		ResetInterpolation();

		SetModel( "models/citizen/citizen.vmdl" );

		//Temporary, should have a custom camera
		CameraMode = new FirstPersonCamera();

		Animator = new StandardPlayerAnimator();

		//Should we have our own walk controller?
		Controller = new WalkController();

		EnableDrawing = true;
		EnableHideInFirstPerson = true;
		EnableShadowInFirstPerson = true;
	}

	public override void Spawn()
	{
		base.Spawn();

		SetUpPlayer();
	}

	public override void Respawn()
	{
		SetUpPlayer();

		//Deletes the corpse if valid
		DestroyCorpse(To.Everyone);
	}

	//Thanks Crayz
	[ClientRpc]
	public void SetViewAngles( Angles angles )
	{
		updateViewAngle = true;
		updatedViewAngle = angles;
	}

	public override void BuildInput( InputBuilder input )
	{
		base.BuildInput( input );

		if ( updateViewAngle )
		{
			updateViewAngle = false;
			input.ViewAngles = updatedViewAngle;
		}
	}

	//Simulation on both server and client
	public override void Simulate( Client cl )
	{
		//We won't call the base simulate since it automatically respawns the player on death in a few seconds
		//but use the last 2 lines of it

		base.Simulate( cl );

		//var controller = GetActiveController();
		//controller?.Simulate( cl, this, GetActiveAnimator() );

		TickPlayerUse();
		SimulatePlacing();

		if ( cl.GetClientData( "cl_showfps", false ) )
		{
			var fps = 1 / RealTime.Delta;
			DebugOverlay.ScreenText( $"{fps.ToString( "0.00" )} FPS", -2 );
		}
	}

	public override void TakeDamage( DamageInfo info )
	{
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

		AchTracker.Update( this, "AfterLife" );
	}

	//Frame simulated on the client
	public override void FrameSimulate( Client cl )
	{
		base.FrameSimulate( cl );
	}

	public override void OnAnimEventFootstep( Vector3 pos, int foot, float volume )
	{
		base.OnAnimEventFootstep( pos, foot, volume * 10 );

		if(IsServer && LifeState == LifeState.Alive)
			AchTracker.Update( this, "Walkathon" );
	}

	public IList<AchBase> GetAchievements()
	{
		IList<AchBase> finished = new List<AchBase>();

		foreach ( var ach in AchTracker.Tracked )
		{
			finished.Add( ach ); 
		}

		return finished;
	}

	//Creates a player ragdoll with clothing (if any) at force with bone index
	[ClientRpc]
	public void CreatePlayerRagdoll( Vector3 force, int forceBone )
	{
		var ent = new ModelEntity();
		ent.Position = Position;
		ent.Rotation = Rotation;
		ent.Scale = Scale;
		ent.UsePhysicsCollision = true;
		ent.EnableAllCollisions = true;

		ent.SetModel( GetModelName() );
		ent.CopyBonesFrom( this );
		ent.CopyBodyGroups( this );
		ent.CopyMaterialGroup( this );
		ent.TakeDecalsFrom( this );
		ent.CopyMaterialOverrides( this );

		ent.EnableHitboxes = true;
		ent.EnableAllCollisions = true;
		ent.SurroundingBoundsMode = SurroundingBoundsType.Physics;
		ent.RenderColor = RenderColor;

		Tags.Clear();
		Tags.Add( "solid" );

		Corpse = ent;

		ent.DeleteAsync( 10.0f );
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
