using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;
using SandboxEditor;

[Library( "ph_suite_teleporter" )]
[Title("Suite Room Teleport"), Description( "Teleport the suite owner (or guests) to their suite" )]
[SupportsSolid]
[HammerEntity]
public partial class SuiteTeleporter : TriggerTeleport
{
	public bool IsLocked = false;

	public bool ClaimedSuite = false;

	public override void Spawn()
	{
		base.Spawn();
	}

	public override void OnTouchStart( Entity other )
	{
		if ( !ClaimedSuite )
			return;

		base.OnTouchStart( other );
	}
}

