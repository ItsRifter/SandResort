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
			InventoryItems = pawn.PHInventory.GetAllItemsString(),
			Achievements = new List<string>(),
			AchProgress = new List<(string, int)>()
		};

		var autoAch = new AchBase();

		autoAch.ServerAutoGiveAchievement( pawn, new NewGuest() );

		autoAch = null;

		FileSystem.Data.WriteJson( cl.PlayerId + ".json", saveData );

	}

	public bool CommitSave(Client cl)
	{
		if ( cl.IsBot )
			return false;

		var pawn = cl.Pawn as PHPawn;

		if ( pawn == null )
			return false;

		List<string> totalAchs = new List<string>();

		List<(string, int)> progressingAchs = new List<(string, int)>();
	
		foreach ( var ach in pawn.AchList )
		{
			totalAchs.Add( ach.GetType().FullName );
		}

		foreach ( var check in pawn.AchChecker )
		{
			progressingAchs.Add( (check.AchName, check.AchProgress) );
		}

		var saveData = new PlayerData()
		{
			PlayerName = cl.Name,
			PlayCoins = pawn.PlayCoins,
			InventoryItems = pawn.PHInventory.GetAllItemsString(),
			Achievements = totalAchs,
			AchProgress = progressingAchs
		};

		FileSystem.Data.WriteJson( cl.PlayerId + ".json", saveData );
		
		return true;
	}

	public bool CommitSuiteSave( Client cl, List<PHSuiteProps> props )
	{
		List<SuitePropInfo> data = new List<SuitePropInfo>();

		foreach ( var prop in props )
		{
			SuitePropInfo suite = new SuitePropInfo()
			{
				PropName = prop.GetType().FullName,
				Model = prop.Model,
				Pos = prop.LocalPosition,
				Rot = prop.LocalRotation,
			};

			data.Add( suite );

			prop.Delete();
		}

		FileSystem.Data.WriteJson( cl.PlayerId + "_suite.json", data );

		return true;
	}

	public bool LoadSuiteSave( Client cl )
	{
		var data = FileSystem.Data.ReadJson<List<SuitePropInfo>>( cl.PlayerId + "_suite.json" );
		
		if ( data == null )
			return false;
		
		var pawn = cl.Pawn as PHPawn;

		if ( pawn == null )
			return false;

		foreach( var item in data.ToArray() )
		{
			var suiteProp = TypeLibrary.Create<PHSuiteProps>( item.PropName );
			
			suiteProp.SetParent( pawn.CurSuite );

			suiteProp.Model = item.Model;
			suiteProp.LocalPosition = item.Pos;
			suiteProp.LocalRotation = item.Rot;
			suiteProp.PropOwner = pawn;

			suiteProp.Spawn();
		}

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

		if ( pawn.AchList == null )
			pawn.AchList = new List<string>();

		foreach ( var ach in data.Achievements )
		{
			if ( pawn.AchList.Contains( ach ) )
				continue;

			pawn.AchList.Add( TypeLibrary.Create<AchBase>( ach ).AchName );
		}


		foreach ( var invItem in data.InventoryItems )
		{
			var item = TypeLibrary.Create(invItem, TypeLibrary.GetTypeByName(invItem)) as Entity;

			pawn.PHInventory.AddItem( item );

			item.Delete();
		}

		return true;
	}
}
