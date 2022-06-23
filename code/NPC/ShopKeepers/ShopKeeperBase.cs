using Sandbox;

public class ShopKeeperBase : AnimatedEntity
{
	public virtual string NPCName => "Base Shop NPC";
	public virtual string ModelPath => "models/citizen/citizen.vmdl";

	public override void Spawn()
	{
		base.Spawn();

		SetModel( ModelPath );

		EyePosition = Position + Vector3.Up * 64;
		CollisionGroup = CollisionGroup.Player;
		SetupPhysicsFromCapsule( PhysicsMotionType.Keyframed, Capsule.FromHeightAndRadius( 72, 8 ) );

		EnableHitboxes = true;
		EnableLagCompensation = true;

		SetBodyGroup( 1, 0 );
	}


	public virtual void InteractWith(LobbyPawn player)
	{
		
	}

	public override void TakeDamage( DamageInfo info )
	{
	}
	public override void OnKilled()
	{
		base.OnKilled();
	}
}
