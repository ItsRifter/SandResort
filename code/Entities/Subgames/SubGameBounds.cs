using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;
using SandboxEditor;

[Library("ph_subgame_bounds")]
[Title("Sub-Game Boundaries"), Category( "Sub-Games" ), Description( "Defines the boundaries of this sub-game play area" )]
[SupportsSolid]
[HammerEntity]
public class SubGameBounds : ModelEntity
{
	public override void Spawn()
	{
		base.Spawn();

		SetupPhysicsFromModel( PhysicsMotionType.Static );
	}
}

