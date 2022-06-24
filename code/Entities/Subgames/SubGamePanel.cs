using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;
using SandboxEditor;

[Library("ph_subgame_panel")]
[Title("Sub-Game Panel"), Category( "Sub-Games" ), Description("The sub-game panel, displays info about this sub-game")]
[BoundsHelper( "MinBounds", "MaxBounds")]
[HammerEntity]
public class SubGamePanel : Entity
{
	[Property]
	public Vector3 MinBounds { get; set; }

	[Property]
	public Vector3 MaxBounds { get; set; }
	
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

		switch(GameType)
		{
			case SubGameType.Unspecified:
				gamePanel.GameName = "Unknown Game, blame the map maker";
				break;
			case SubGameType.Monday_Massacre:
				gamePanel.GameName = "Monday Massacre";
				break;
		}
		

		gamePanel.Position = Position;
		gamePanel.Rotation = Rotation;

		gamePanel.TriggerBox = new BBox( MinBounds, MaxBounds );
	}
}

