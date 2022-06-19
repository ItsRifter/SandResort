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

		if ( !IsServer )
			return;

		if ( player.CurSuite != null )
		{
			CheckOut( player );
			return;
		}

		CheckIn( player );
	}

	public void CheckIn( PHPawn player )
	{
		var curSuites = All.OfType<SuiteRoomEnt>().ToArray();

		SuiteRoomEnt randomSuite = null;

		randomSuite = curSuites.OrderBy( x => Guid.NewGuid() ).FirstOrDefault( x => x.SuiteOwner == null );

		if(IsServer)
			ConsoleSystem.Run( "ph_server_say", $"You have checked into suite {randomSuite.Name.Substring( 6 )}", player.Client.Id );

		Log.Info( $"{player.Client.Name} checked into {randomSuite.Name}" );

		player.CurSuite = randomSuite;
		player.CurSuite.SuiteOwner = player;
		player.CurSuite.SuiteTele.ClaimedSuite = true;

		PHGame.Instance.LoadSuiteSave( player.Client );
	}

	public void CheckOut( PHPawn player )
	{
		player.CurSuite.RevokeSuite( player );

		if ( IsServer )
			ConsoleSystem.Run( "ph_server_say", "You have checked out of your suite", player.Client.Id );

		Log.Info( $"{player.Client.Name} checked out" );
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

