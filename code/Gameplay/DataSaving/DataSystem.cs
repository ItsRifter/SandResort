using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;

public class PlayerStatSaveable : IPlayerStat
{
	public int PlayCoins { get; set; }
	public string PlayerName { get; }
}

public partial class PHGame
{
	public void NewPlayer(Client cl)
	{
		var pawn = cl.Pawn as PHPawn;

		pawn.SetCoins( 500 );

		FileSystem.Data.WriteJson( cl.PlayerId + ".json", (IPlayerStat) pawn );

	}

	public bool CommitSave(Client cl)
	{
		var pawn = cl.Pawn as PHPawn;

		FileSystem.Data.WriteJson( cl.PlayerId + ".json", (IPlayerStat) pawn );
		
		return true;
	}

	public bool LoadSave(Client cl)
	{
		var data = FileSystem.Data.ReadJson<PlayerStatSaveable>( cl.PlayerId + ".json" );

		if ( data == null )
			return false;

		var pawn = cl.Pawn as PHPawn;

		if ( pawn == null )
			return false;

		pawn.SetCoins(data.PlayCoins);

		return true;
	}
}
