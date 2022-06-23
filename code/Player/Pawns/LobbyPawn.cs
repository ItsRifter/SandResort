using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;


public partial class LobbyPawn : BasePawn
{
	[Net]
	public Entity InteractNPC { get; set; }

	public float Drunkiness;
	public TimeSince TimeLastDrank;

	public PHSittableProp SitProp;

	public TimeSince timeLastRespawn;

	TimeSince timeTillSober;

	//Don't allow players to spam death and respawning
	TimeSince timeLastDied;

	DamageInfo lastDMGInfo;

	public LobbyPawn()
	{
		PHInventory = new PHInventorySystem(this);
		CreateClientInventory();
		CurPlayers = new List<LobbyPawn>();
	}

	public void Test(TeleDest test)
	{
		Transform = test.Transform;
		EyePosition = test.Rotation.Forward;
	}

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

		Tags.Add( "PH_Player" );

		Drunkiness = 0.0f;

		if( AchList == null )
			AchList = new List<AchBase>();

		if ( AchChecker == null )
			AchChecker = new List<AchBase>();

		//Use the base player respawn, NOT the respawn in this class
		Host.AssertServer();

		LifeState = LifeState.Alive;
		Health = 100;
		Velocity = Vector3.Zero;
		WaterLevel = 0;

		CreateHull();

		PHGame.Instance?.MoveToSpawnpoint( this );
		ResetInterpolation();

		SetInteractsAs( CollisionLayer.Player );
		SetInteractsExclude( CollisionLayer.Player );
	}

	public override void Respawn()
	{
		base.Respawn();

		SetInteractsAs( CollisionLayer.Player );
		SetInteractsExclude( CollisionLayer.Player );

		CameraMode = new FirstPersonCamera();

		EnableAllCollisions = true;
		EnableDrawing = true;

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

		Drunkiness = 0.0f;

		timeLastRespawn = 0;

		if ( ActiveChildren == null )
			ActiveChildren = new List<Entity>();
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

		if ( LifeState == LifeState.Alive )
			SimulateActions();
		else if ( LifeState == LifeState.Dead )
			SimulateActionsWhilstDead();

		foreach ( var child in ActiveChildren)
		{
			if(child.IsAuthority)
				child.Simulate( cl );
		}

	}

	TimeSince jumpingJackCooldown;

	void SimulateActions()
	{
		if ( IsServer )
		{
			if ( InteractNPC is SuiteReceptionist suiteRecep && Input.Pressed(InputButton.Use) )
				suiteRecep.InteractWith( this );

			if ( InteractNPC != null && Position.Distance( InteractNPC.Position ) > 250 )
				InteractNPC = null;
		
			if(Input.Pressed(InputButton.Jump) && jumpingJackCooldown > 0.85f )
			{
				jumpingJackCooldown = 0;

				CheckOrUpdateAchievement( "Jumping Jacks", "JumpingJacks" );
			}
			

			TickPlayerUse();

			InteractNPC = FindNPC();
		}

		SimulatePropPlacement();

		if ( Drunkiness > 0.0f )
			SimulateDrunkState();
	}

	void SimulateDrunkState()
	{
		if ( TimeLastDrank < 7.5f )
			return;

		if ( timeTillSober < 2.5f )
			return;

		Drunkiness -= 2.5f;
		timeTillSober = 0;
	}

	protected override void TickPlayerUse()
	{
		if ( !Host.IsServer ) return;

		using ( Prediction.Off() )
		{
			if ( Input.Pressed( InputButton.Use ) )
			{
				Using = FindUsable();
				
				if ( Using == null )
				{
					return;
				}
			}

			if ( Using is PHSuiteProps prop )
			{
				prop.Interact( this );
				Using = null;
				return;
			}

			if(Using is Poker pokerTable)
			{
				pokerTable.Interact( this );
				Using = null;
				return;
			}	

			if ( !Input.Down( InputButton.Use ) )
			{
				StopUsing();
				return;
			}

			if ( !Using.IsValid() )
				return;

			if ( Using is IUse use && use.OnUse( this ) )
				return;

			StopUsing();
		}
	}

	protected override Entity FindUsable()
	{
		var tr = Trace.Ray( EyePosition, EyePosition + EyeRotation.Forward * 125 )
			.Ignore( this )
			.Run();

		if ( tr.Entity is PHSuiteProps prop )
			return prop;

		if ( tr.Entity is Poker poker )
			return poker;

		return base.FindUsable();
	}

	protected Entity FindNPC()
	{
		var tr = Trace.Ray( EyePosition, EyePosition + EyeRotation.Forward * 125 )
			.Ignore( this )
			.Run();

		if ( tr.Entity is PHBaseNPC baseNPC )
			return baseNPC;

		if ( tr.Entity is ShopKeeperBase shopNPC )
			return shopNPC;

		return null;
	}
	void SimulateActionsWhilstDead()
	{
		if(IsServer)
		{
			if(Input.Pressed(InputButton.PrimaryAttack) && timeLastDied > 3.0f)
				Respawn();
		}
	}

	[ClientRpc]
	public void CreateClientInventory()
	{
		PHInventory.ClientInventory = new List<string>();
	}

	[ClientRpc]
	public void UpdateClientInventory( string newItem, bool shouldAdd = true )
	{
		if ( shouldAdd )
			PHInventory.ClientInventory.Add( newItem );
		else if ( !shouldAdd )
			PHInventory.ClientInventory.Remove(newItem);
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

		if(CurSuite != null)
		{
			CurSuite.RevokeSuite( this );

			//if ( IsServer )
				//ConsoleSystem.Run( "ph_server_say", "You were automatically checked out of your suite", Client.Id );
		}

		//We should make a first person death camera in the future
		CameraMode = new RagdollCamera();

		timeLastDied = 0;
	}

	//Frame simulated on the client
	public override void FrameSimulate( Client cl )
	{
		base.FrameSimulate( cl );
	}
}
