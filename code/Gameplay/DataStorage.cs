using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;

interface IPlayerData
{
	string PlayerName { get; }
	IList<AchBase> Achievements { get; }
}

public partial class BasePawn : IPlayerData
{
	public string PlayerName { get => Client.Name; }
	public IList<AchBase> Achievements { get => GetAchievements(); }
}

public partial class SCGame
{
	public void SavePlayer(BasePawn player)
	{
		FileSystem.Data.WriteJson($"{player.Client.PlayerId}.json", (IPlayerData)player);
	}

	public bool LoadPlayer( BasePawn player )
	{
		if ( !FileSystem.Data.FileExists( $"{player.Client.PlayerId}.json" ) )
			return false;

		var data = FileSystem.Data.ReadJson<IPlayerData>( $"{player.Client.PlayerId}.json" );

		foreach ( AchBase ach in data.Achievements )
			player.AchTracker.Tracked.Add( ach );


		return true;
	}
}

