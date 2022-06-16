using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;

public partial class SuiteReceptionist : PHBaseNPC
{
	public override string NPCName => "Suite Receptionist";
	public override string ModelPath => "models/citizen/citizen.vmdl";

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

	public override void InteractWith( PHPawn player )
	{
		base.InteractWith( player );

		if ( player.CurSuite != null )
		{
			CheckOut( player );
			return;
		}

		CheckIn( player );
	}

	int attempts = 2;

	public void CheckIn( PHPawn player )
	{
		var curSuites = All.OfType<SuiteRoomEnt>();

		SuiteRoomEnt randomSuite = null;
		int tries = 0;

		randomSuite = curSuites.OrderBy( x => Guid.NewGuid() ).FirstOrDefault();

		while (randomSuite.SuiteOwner == null)
		{
			if ( attempts >= tries )
				break;

			randomSuite = curSuites.OrderBy( x => Guid.NewGuid() ).FirstOrDefault();
			
			if ( randomSuite.SuiteOwner != null )
				break;

			tries++;
		}

		if ( randomSuite == null )
			return;

		Log.Info( $"{player.Client.Name} checked into {randomSuite.Name}" );

		player.CurSuite = randomSuite;
		player.CurSuite.SuiteTele.ClaimedSuite = true;
	}

	public void CheckOut( PHPawn player )
	{
		Log.Info( $"{player.Client.Name} checked out" );

		player.CurSuite.SuiteTele.ClaimedSuite = false;
		player.CurSuite = null;
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

