using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;

public partial class PHGame
{

	//Creates a new player save
	public void NewPlayer(Client cl)
	{
		var pawn = cl.Pawn as LobbyPawn;

		pawn.SetCoins( 500 );

		var saveData = new PlayerData()
		{
			PlayerName = cl.Name,
			PlayCoins = pawn.PlayCoins,
			InventoryItems = pawn.PHInventory.GetAllItemsString(),
			Achievements = new List<AchData>(),
			SuiteProps = new List<SuitePropInfo>()
		};

		var autoAch = new NewGuest();
		autoAch.HasCompleted = true;
		pawn.PlaySound( autoAch.AchUnlockSound );
		pawn.AchList.Add( autoAch );

		ConsoleSystem.Run( "say", $"{pawn.Client.Name} has earned the achievement: {autoAch.AchName}", true );

		autoAch = null;

		FileSystem.Data.WriteJson($"{cl.PlayerId}.json", saveData );

	}

	//Normal saving
	public bool CommitSave(Client cl, List<PHSuiteProps> props = null)
	{
		if ( cl.IsBot )
			return false;

		var pawn = cl.Pawn as LobbyPawn;

		if ( pawn == null )
			return false;

		List<AchData> achDataList = new List<AchData>();

		foreach ( var ach in pawn.AchChecker)
		{
			AchData achData = new AchData();

			achData.AchievementName = ach.AchName;
			achData.AchievementClass = ach.GetType().FullName;
			achData.AchievementProgress = ach.AchProgress;
			achData.IsCompleted = ach.HasCompleted;

			achDataList.Add( achData );
		}

		List<SuitePropInfo> dataProps = new List<SuitePropInfo>();

		//If we're updating player's suite props
		if ( props != null )
		{
			foreach ( var prop in props )
			{
				SuitePropInfo suite = new SuitePropInfo()
				{
					PropName = prop.ClassName,
					Model = prop.Model,
					Pos = prop.LocalPosition,
					Rot = prop.LocalRotation
				};

				dataProps.Add( suite );

				prop.Delete();
			}
		} 
		else
		{
			//Get the last suite props if we aren't updating this
			dataProps = FileSystem.Data.ReadJson<PlayerData>( $"{cl.PlayerId}.json" ).SuiteProps;
		}

		var saveData = new PlayerData()
		{
			PlayerName = cl.Name,
			PlayCoins = pawn.PlayCoins,
			InventoryItems = pawn.PHInventory.GetAllItemsString(),
			Achievements = achDataList,
			SuiteProps = dataProps
		};

		FileSystem.Data.WriteJson( $"{cl.PlayerId}.json", saveData );
		
		return true;
	}

	//Suite loading, we won't load other player stats but their suite
	public bool LoadSuiteSave( Client cl )
	{
		var data = FileSystem.Data.ReadJson<PlayerData>( $"{cl.PlayerId}.json" );
		
		if ( data == null )
			return false;

		if ( data.SuiteProps == null )
			return false;

		if ( cl.Pawn is not LobbyPawn pawn )
			return false;

		foreach( SuitePropInfo item in data.SuiteProps.ToArray() )
		{
			PHSuiteProps suiteProp = TypeLibrary.Create<PHSuiteProps>( item.PropName );
			
			suiteProp.SetParent( pawn.CurSuite );

			suiteProp.Model = item.Model;
			suiteProp.LocalPosition = item.Pos;
			suiteProp.LocalRotation = item.Rot;
			suiteProp.PropOwner = pawn;

			suiteProp.Spawn();
		}

		return true;
	}

	//Normal loading
	public bool LoadSave(Client cl)
	{
		var data = FileSystem.Data.ReadJson<PlayerData>( $"{cl.PlayerId}.json" );

		if ( data == null )
			return false;

		var pawn = cl.Pawn as LobbyPawn;

		if ( pawn == null )
			return false;

		pawn.SetCoins(data.PlayCoins);

		foreach ( var invItem in data.InventoryItems )
		{
			Entity item = TypeLibrary.Create(invItem, TypeLibrary.GetTypeByName(invItem)) as Entity;

			pawn.PHInventory.AddItem( item );

			pawn.UpdateClientInventory(item.ClassName);

			item.Delete();
		}

		pawn.SetCoins( data.PlayCoins );

		List<AchBase> achLoadData = new List<AchBase>();

		foreach ( var ach in data.Achievements )
		{
			if ( achLoadData.Find( x => x.AchName == ach.AchievementName ) != null )
				continue;

			AchBase achLoad = TypeLibrary.Create<AchBase>( ach.AchievementClass );
			achLoad.AchProgress = ach.AchievementProgress;
			achLoad.HasCompleted = ach.IsCompleted;

			achLoadData.Add( achLoad );
		}

		pawn.AchList = achLoadData;
		pawn.AchChecker = achLoadData;

		return true;
	}
}
