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

public partial class PlayerData : IPlayerData
{
	public string PlayerName { get; }
	public IList<AchBase> Achievements { get; set; }
}

public partial class SCGame
{
	public void SavePlayer(Client cl)
	{
		var pawn = cl.Pawn as BasePawn;

		if ( pawn == null )
			return;

		FileSystem.Data.WriteJson($"{cl.PlayerId}.json", (IPlayerData)pawn );
	}

	public bool LoadPlayer( Client cl )
	{
		if ( !FileSystem.Data.FileExists( $"{cl.PlayerId}.json" ) )
			return false;

		var data = FileSystem.Data.ReadJson<PlayerData>( $"{cl.PlayerId}.json" );

		var pawn = cl.Pawn as BasePawn;

		if ( pawn == null )
			return false;

		foreach ( var ach in data.Achievements )
		{
			Log.Info( ach.AchName );
			//pawn.AchTracker.Tracked.Add( ach );
		}

		return true;
	}
}



