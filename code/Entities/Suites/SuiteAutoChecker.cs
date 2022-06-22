using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;
using SandboxEditor;

[Library( "ph_suite_checker" )]
[Title("Suite Checker"), Category( "Suites" ), Description( "Checks if the player is in a suite, if so check them out" )]
[SupportsSolid]
[HammerEntity]
public partial class SuiteAutoChecker : TriggerMultiple
{

	public override void Spawn()
	{
		base.Spawn();
	}

	public override void OnTouchStart( Entity other )
	{
		if(other is LobbyPawn player && player.CurSuite != null)
		{
			player.CurSuite.RevokeSuite( player );
			Log.Info( $"{player.Client.Name} was automatically checked out by leaving" );

			//if ( IsServer )
			//	ConsoleSystem.Run( "ph_server_say", "You were automatically checked out of your suite", player.Client.Id );
		}

		base.OnTouchStart( other );
	}
}

