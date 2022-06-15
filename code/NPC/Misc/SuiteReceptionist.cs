using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;

public partial class SuiteReceptionist : AnimatedEntity
{
	public virtual string NPCName => "Suite Receptionist";
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

	public void Interact( PHPawn player )
	{
		//Do checking in stuff
	}

	public void CheckIn( PHPawn player )
	{
		//Do checking in stuff
	}

	public void CheckOut( PHPawn player )
	{
		//Do checking out stuff
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

