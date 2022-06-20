using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;
using SandboxEditor;

[Library( "ph_trigger_teleport" )]
[Title("PlayHome Trigger Teleport"), Description( "A modified version of trigger teleport" )]
[SupportsSolid]
[HammerEntity]
public partial class PHTriggerTeleport : TriggerTeleport
{
	public override void Spawn()
	{
		base.Spawn();
	}

	public override void OnTouchStart( Entity other )
	{
		if ( !Enabled ) return;

		var Targetent = FindByName( TargetEntity );

		if ( Targetent != null )
		{
			Vector3 offset = Vector3.Zero;

			if ( TeleportRelative )
			{
				offset = other.Position - Position;
			}

			if ( !KeepVelocity ) other.Velocity = Vector3.Zero;

			// Fire the output, before actual teleportation so entity IO can do things like disable a trigger_teleport we are teleporting this entity into
			OnTriggered.Fire( other );

			other.Transform = Targetent.Transform;
			other.Rotation = Targetent.Rotation;
			other.Position += offset;
		}
	}
}

