using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;
using SandboxEditor;

[Library( "sc_subgame_mm" )]
[Title( "Monday Massacre World Panel" ), Category( "Sub-Games" ), Description( "The Monday Massacre Panel" )]
[HammerEntity]

public class MMPanel : SubGamePanel
{
	public override SubGameType GameType => SubGameType.Monday_Massacre;

	public override void Spawn()
	{
		base.Spawn();
	}

	public override void ClientSpawn()
	{
		base.ClientSpawn();
	}

}
