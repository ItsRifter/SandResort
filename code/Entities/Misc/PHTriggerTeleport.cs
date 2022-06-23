using Sandbox;
using SandboxEditor;

[Library( "ph_trigger_teleport" )]
[Title("PlayHome Trigger Teleport"), Category("Misc"), Description( "A modified version of trigger teleport" )]
[SupportsSolid]
[HammerEntity]
public class PHTriggerTeleport : TriggerTeleport
{

	public override void Spawn()
	{
		base.Spawn();
	}


	public override void OnTouchStart( Entity other )
	{
		if ( !Enabled ) return;

		base.OnTouchStart(other);
		
		if ( other is LobbyPawn player )
			player.SetViewAngles( To.Single( player ), FindByName(TargetEntity).Rotation.Angles() );
	}
}

