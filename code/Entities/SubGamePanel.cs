using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;
using SandboxEditor;

[Library("ph_subgame_panel")]
[Title("Sub-Game Panel"), Description("The sub-game panel, displays info about this sub-game")]
[HammerEntity]
public class SubGamePanel : Entity
{

	public enum SubGameType
	{
		Unspecified,
		Monday_Massacre
	}

	[Property]
	public SubGameType GameType { get; set; } = SubGameType.Unspecified;

	GameWorldPanel gamePanel;
	public override void Spawn()
	{
		base.Spawn();
	}

	public override void ClientSpawn()
	{
		base.ClientSpawn();

		gamePanel = new GameWorldPanel();

		gamePanel.Position = Position;
		gamePanel.Rotation = Rotation;

		//TEMPORARY, should be set by the SubGamePanel entity bounding box
		gamePanel.TriggerBox = new BBox( gamePanel.Position / 1.01f, gamePanel.Position * 1.01f );
	}
}

