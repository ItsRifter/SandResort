using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;
using SandboxEditor;

[Library("sc_trigger_condoteleport")]
[Title("Condo Teleport"), Description("Simply the trigger teleport modified for condo entering/leaving"), Category("Triggers")]
[HammerEntity, Solid]
public partial class CondoTeleporter : TeleportVolumeEntity
{
	public Entity CondoTarget { get; set; }

	protected Output OnEnterTrigger { get; set; }

	public override void OnTouchStart( Entity other )
	{
		if ( !Enabled ) return;

		if ( other is not BasePawn player )
			return;

		var Targetent = CondoTarget;

		if ( Targetent != null )
		{
			Vector3 offset = Vector3.Zero;
			if ( TeleportRelative )
			{
				offset = player.Position - Position;
			}

			if ( !KeepVelocity ) player.Velocity = Vector3.Zero;

			OnEnterTrigger.Fire( player );

			player.Transform = Targetent.Transform;
			player.Position += offset;
			player.SetViewAngles(Targetent.Rotation.Angles());
		}
	}
}

