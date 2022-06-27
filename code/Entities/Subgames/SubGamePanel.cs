using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;

public partial class SubGamePanel : Entity
{
	public enum SubGameType
	{
		Unspecified,
		Monday_Massacre
	}

	public virtual SubGameType GameType { get; set; } = SubGameType.Unspecified;

	public GameWorldPanel gamePanel { get; set; }

	public override void Spawn()
	{
		base.Spawn();
	}

	public override void ClientSpawn()
	{
		base.ClientSpawn();

		gamePanel = new GameWorldPanel();

		Log.Info( GameType );

		gamePanel.Position = Position;
		gamePanel.Rotation = Rotation;
	}
}

