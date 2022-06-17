using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;
using SandboxEditor;

[Library( "ph_suite_checker" )]
[Title("Suite Checker"), Description( "Checks if the player is in a suite, if so check them out" )]
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
		if(other is PHPawn player && player.CurSuite != null)
		{
			player.CurSuite.RevokeSuite( player );
			Log.Info( $"{player.Client.Name} was automatically checked out by leaving" );
		}

		base.OnTouchStart( other );
	}
}

