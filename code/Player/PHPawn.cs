using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;


public partial class PHPawn : Player
{
	[Net, Predicted]
	public IList<Entity> ActiveChildren { get; set; }

	[Net]
	public bool OpenShop { get; set; } = false;

	[Net]
	public ShopKeeperBase ShopKeeper { get; set; }

	public TimeSince timeLastRespawn;

	DamageInfo lastDMGInfo;

	//Don't allow players to spam death and respawning
	TimeSince timeLastDied;

	public PHInventorySystem PHInventory;

	public float Drunkiness;
	public TimeSince TimeLastDrank;
	TimeSince timeTillSober;

	public PHPawn()
	{
		PHInventory = new PHInventorySystem(this);
		CreateClientInventory();
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

	Entity NPCInteration = null;
	TimeSince timeToInteract;

	void SimulateActions()
	{
		if ( IsServer )
		{
			TickPlayerUse();

			NPCInteration = FindNPC();

			if( NPCInteration is SuiteReceptionist normalNPC && Input.Pressed( InputButton.Use ) )
			{
				if ( timeToInteract <= 1 )
					return;

				normalNPC.InteractWith(this);

				timeToInteract = 0;
			} 

			if ( NPCInteration is ShopKeeperBase NPC && !OpenShop )
			{
				if (Input.Pressed(InputButton.Use) )
				{
					NPC.InteractWith( this );
					return;
				}
			} else if (OpenShop && Input.Pressed( InputButton.Use ) )
			{
				OpenShop = false;
				ShopKeeper = null;
			}

			SimulatePropPlacement();
		}
		
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
				
				//NPCInteration = FindNPC();
				
				if ( Using == null )
				{
					return;
				}
			}

/*			if ( NPCInteration is PHBaseNPC NPC )
			{
				NPC.InteractWith( this );
				NPCInteration = null;
				return;
			}*/

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
		PHInventory.ClientInventory = new List<(string, string)>();
	}

	[ClientRpc]
	public void UpdateClientInventory( string newItem, string itemImage, bool shouldAdd = true )
	{
		if ( shouldAdd )
			PHInventory.ClientInventory.Add( (newItem, itemImage) );
		else if ( !shouldAdd )
			PHInventory.ClientInventory.Remove( (newItem, itemImage) );
	}

	public override void TakeDamage( DamageInfo info )
	{
		if ( info.Attacker is PHPawn )
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

	public override void OnAnimEventFootstep( Vector3 pos, int foot, float volume )
	{
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
