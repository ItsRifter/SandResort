using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;
using SandboxEditor;

[Library( "ph_trigger_teleport" )]
[Title("PlayHome Trigger Teleport"), Category("Misc"), Description( "A modified version of trigger teleport" )]
[SupportsSolid]
[HammerEntity]
public class PHTriggerTeleport : TeleportVolumeEntity
{
	public override void Spawn()
	{
		base.Spawn();
	}

	public override void OnTouchStart( Entity other )
	{
		if ( !Enabled ) return;

		base.OnTouchStart(other);

		if ( other is BasePawn player )
		{
			player.SetViewAngles( To.Single( player ), FindByName( TargetEntity.ToString() ).Rotation.Angles() );
			player.ResetInterpolation();
		}
	}
}

