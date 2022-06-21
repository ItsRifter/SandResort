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

	public override void InteractWith( LobbyPawn player )
	{
		base.InteractWith( player );

		if ( !IsServer )
			return;

		if ( player.Drunkiness > 0 )
			player.CheckOrUpdateAchievement( "Best pickup line", "PickupLine" );

		/*if ( player.CurSuite != null )
		{
			CheckOut( player );
			return;
		}

		if(!CheckIn(player))
			ConsoleSystem.Run( "ph_server_say", "There are no suites available at this time", player.Client.Id );*/
	}

	[ConCmd.Server("ph_claim_suite")]
	public static void ClaimSuiteStatic(int suiteIndex, int plyID)
	{
		var player = ConsoleSystem.Caller.Pawn as LobbyPawn;

		if ( player == null || player.Client.Id != plyID )
			return;

		if ( player.CurSuite != null )
			return;

		var curSuites = All.OfType<SuiteRoomEnt>().ToArray();

		SuiteRoomEnt randomSuite = null;

		randomSuite = curSuites[suiteIndex];

		//If there is no suite available or suite is already claimed, stop here
		if ( randomSuite == null || randomSuite.SuiteOwner != null )
			return;

		//ConsoleSystem.Run( "ph_server_say", $"You have checked into suite {randomSuite.Name.Substring( 6 )}", player.Client.Id );

		Log.Info( $"{player.Client.Name} checked into {randomSuite.Name}" );

		player.CurSuite = randomSuite;
		player.CurSuite.SuiteOwner = player;
		player.CurSuite.SuiteTeleporter.Enable();

		PHGame.Instance.LoadSuiteSave( player.Client );
	}

	//Other NPC stuff
	public override void TakeDamage( DamageInfo info )
	{
		return;
	}

	public override void OnKilled()
	{
		base.OnKilled();
	}
}

