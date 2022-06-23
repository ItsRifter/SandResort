using System.Collections.Generic;
using Sandbox;

public partial class ShopKeeperBase : AnimatedEntity
{
	public virtual string NPCName => "Base Shop NPC";
	public virtual string ModelPath => "models/citizen/citizen.vmdl";
	public virtual List<string> ClothingModels => new List<string>
	{
		//Here should be clothing models for this shop keeper
	};

	public override void Spawn()
	{
		base.Spawn();

		SetModel( ModelPath );

		CollisionGroup = CollisionGroup.Player;
		SetupPhysicsFromCapsule( PhysicsMotionType.Keyframed, Capsule.FromHeightAndRadius( 72, 8 ) );

		EnableHitboxes = true;
		EnableLagCompensation = true;

		SetBodyGroup( 1, 0 );

		foreach ( var clothModel in ClothingModels )
		{
			var clothing = new ModelEntity();

			clothing.SetModel( clothModel );
			clothing.SetParent( this, true );

			clothing.Spawn();
		}
	}


	public virtual void InteractWith(LobbyPawn player)
	{
		
	}

	public override void TakeDamage( DamageInfo info )
	{
		return;
	}
	public override void OnKilled()
	{
		base.OnKilled();
	}
}
