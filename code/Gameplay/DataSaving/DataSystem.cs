using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;

public partial class PHGame
{
	public void NewPlayer(Client cl)
	{
		var pawn = cl.Pawn as PHPawn;

		pawn.SetCoins( 500 );

		var saveData = new PlayerData()
		{
			PlayerName = cl.Name,
			PlayCoins = pawn.PlayCoins,
			InventoryItems = pawn.PHInventory.GetAllItemsString()
		};

		FileSystem.Data.WriteJson( cl.PlayerId + ".json", saveData );

	}

	public bool CommitSave(Client cl)
	{
		var pawn = cl.Pawn as PHPawn;


		var saveData = new PlayerData()
		{
			PlayerName = cl.Name,
			PlayCoins = pawn.PlayCoins,
			InventoryItems = pawn.PHInventory.GetAllItemsString()
		};

		FileSystem.Data.WriteJson( cl.PlayerId + ".json", saveData );
		
		return true;
	}

	public bool LoadSave(Client cl)
	{
		var data = FileSystem.Data.ReadJson<PlayerData>( cl.PlayerId + ".json" );

		if ( data == null )
			return false;

		var pawn = cl.Pawn as PHPawn;

		if ( pawn == null )
			return false;

		pawn.SetCoins(data.PlayCoins);

		foreach ( var invItem in data.InventoryItems )
		{
			var item = TypeLibrary.Create(invItem, TypeLibrary.GetTypeByName(invItem)) as Entity;

			pawn.PHInventory.AddItem( item );

			item.Delete();
		}

		return true;
	}
}
