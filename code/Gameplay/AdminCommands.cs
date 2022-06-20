using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;
public partial class PHGame
{
	//Hardcoded for now until we can figure out an websocket admin way

	[Net]
	public IList<long> AdminList { get; protected set; }

	static float eyeDist = 150.0f;

	[ConCmd.Server( "ph_coins_give" )]
	public static void AdminGiveCoins( int amount, string target = "" )
	{
		if ( Instance.AdminList == null || !Instance.AdminList.Contains( ConsoleSystem.Caller.PlayerId ) )
		{
			Log.Error( "You do not have access to this command" );
			return;
		}

		if ( amount <= 0 || amount >= 10000000 )
		{
			Log.Error( "Invalid amount" );
			return;
		}

		var player = ConsoleSystem.Caller.Pawn as LobbyPawn;

		if ( player == null )
			return;

		if ( string.IsNullOrEmpty( target ) )
		{
			Log.Info( $"{player.Client.Name} gave themself {amount} PlayCoins" );
			player.GiveCoins( amount );
		}
		else
		{
			LobbyPawn targetPlayer = null;

			foreach ( var client in Client.All )
			{
				if ( client.Name.ToLower().Contains( target ) )
				{
					if ( targetPlayer != null )
					{
						Log.Error( "There are multiple targets with this name, be more specific" );
						return;
					}
					else
					{
						targetPlayer = client.Pawn as LobbyPawn;
					}
				}
			}

			if ( targetPlayer != null )
			{
				Log.Info( $"{player.Client.Name} gave {targetPlayer.Client.Name} {amount} PlayCoins" );
				targetPlayer.GiveCoins( amount );
			}
			else
			{
				Log.Error( "No target found" );
			}
		}
	}

	[ConCmd.Server( "ph_coins_take" )]
	public static void AdminTakeCoins( int amount, string target = "" )
	{
		if ( amount <= 0 || amount >= 10000000 )
		{
			Log.Error( "Invalid amount" );
			return;
		}

		if ( Instance.AdminList == null || !Instance.AdminList.Contains( ConsoleSystem.Caller.PlayerId ) )
		{
			Log.Error( "You do not have access to this command" );
			return;
		}

		var player = ConsoleSystem.Caller.Pawn as LobbyPawn;

		if ( player == null )
			return;

		if ( string.IsNullOrEmpty( target ) )
		{
			Log.Info( $"{player.Client.Name} removed {amount} PlayCoins from themselves" );
			player.TakeCoins( amount );
		}
		else
		{
			LobbyPawn targetPlayer = null;

			foreach ( var client in Client.All )
			{
				if ( client.Name.ToLower().Contains( target ) )
				{
					if ( targetPlayer != null )
					{
						Log.Error( "There are multiple targets with this name, be more specific" );
						return;
					}
					else
					{
						targetPlayer = client.Pawn as LobbyPawn;
					}
				}
			}

			if ( targetPlayer != null )
			{
				Log.Info( $"{player.Client.Name} removed {amount} PlayCoins from {targetPlayer.Client.Name}" );
				targetPlayer.TakeCoins( amount );
			}
			else
			{
				Log.Error( "No target found" );
			}
		}
	}

	[ConCmd.Server( "ph_spawn_item_suite" )]
	public static void AdminSpawnItem( string itemSuiteName )
	{
		if ( Instance.AdminList == null || !Instance.AdminList.Contains( ConsoleSystem.Caller.PlayerId ) )
		{
			Log.Error( "You do not have access to this command" );
			return;
		}

		if ( TypeLibrary.GetTypeByName<PHSuiteProps>( itemSuiteName ) == null )
			return;

		var player = ConsoleSystem.Caller.Pawn as LobbyPawn;

		if ( player == null )
			return;

		var tr = Trace.Ray( player.EyePosition, player.EyePosition + player.EyeRotation.Forward * eyeDist )
			.Ignore( player )
			.Run();

		var itemSuite = TypeLibrary.Create<PHSuiteProps>( itemSuiteName );

		itemSuite.Rotation = player.Rotation.Inverse;
		itemSuite.Position = tr.EndPosition;
		itemSuite.Spawn();
	}

	[ConCmd.Server( "ph_spawn_npc" )]
	public static void AdminSpawnNPC( string npcName )
	{
		if ( Instance.AdminList == null || !Instance.AdminList.Contains( ConsoleSystem.Caller.PlayerId ) )
		{
			Log.Error( "You do not have access to this command" );
			return;
		}

		if ( TypeLibrary.GetTypeByName<ShopKeeperBase>( npcName ) == null )
			return;

		var player = ConsoleSystem.Caller.Pawn as LobbyPawn;

		if ( player == null )
			return;

		var tr = Trace.Ray( player.EyePosition, player.EyePosition + player.EyeRotation.Forward * eyeDist )
			.Ignore( player )
			.Run();

		var npc = TypeLibrary.Create<ShopKeeperBase>( npcName );

		npc.Position = tr.EndPosition;
		npc.Spawn();
	}

	[ConCmd.Server( "ph_item_give" )]
	public static void AdminGiveItem( string itemName, string target = "" )
	{
		if ( !Instance.AdminList.Contains( ConsoleSystem.Caller.PlayerId ) )
			return;

		var player = ConsoleSystem.Caller.Pawn as LobbyPawn;

		if ( player == null )
			return;

		if ( TypeLibrary.GetTypeByName<PHUsableItemBase>( itemName ) == null )
			return;

		var item = TypeLibrary.Create<PHUsableItemBase>( itemName );

		if ( string.IsNullOrEmpty( target ) )
		{
			Log.Info( $"{player.Client.Name} gave item {item.GetType().FullName} to themselves" );
			player.PHInventory.AddItem( item );
		}
		else
		{
			LobbyPawn targetPlayer = null;

			foreach ( var client in Client.All )
			{
				if ( client.Name.ToLower().Contains( target ) )
				{
					if ( targetPlayer != null )
					{
						Log.Error( "There are multiple targets with this name, be more specific" );
						return;
					}
					else
					{
						targetPlayer = client.Pawn as LobbyPawn;
					}
				}
			}

			if ( targetPlayer != null )
			{
				Log.Info( $"{player.Client.Name} gave item {item.GetType().FullName} to {targetPlayer.Client.Name}" );
				targetPlayer.PHInventory.AddItem( item );
			}
			else
			{
				Log.Error( "No target found" );
			}
		}
	}

	[ConCmd.Server( "ph_data_save" )]
	public static void AdminSaveData( string target = "" )
	{
		var caller = ConsoleSystem.Caller;

		if ( caller == null )
			return;

		if (string.IsNullOrEmpty(target))
		{
			Instance.CommitSave( caller );
		}
		else
		{
			Client targetClient = null;

			foreach ( var client in Client.All )
			{
				if ( client.Name.ToLower().Contains( target ) )
				{
					if ( targetClient != null )
					{
						Log.Error( "There are multiple targets with this name, be more specific" );
						return;
					}
					else
					{
						targetClient = client;
					}
				}
			}

			if ( targetClient != null )
			{
				Log.Info( $"{caller.Name} forced a data save on {targetClient.Name}" );
				Instance.CommitSave( targetClient );
			}
			else
			{
				Log.Error( "No target found" );
			}
		}
	}

	[ConCmd.Server( "ph_data_load" )]
	public static void AdminLoadData( string target = "" )
	{
		var caller = ConsoleSystem.Caller;

		if ( caller == null )
			return;

		if ( string.IsNullOrEmpty( target ) )
		{
			Instance.LoadSave( caller );
		}
		else
		{
			Client targetClient = null;

			foreach ( var client in Client.All )
			{
				if ( client.Name.ToLower().Contains( target ) )
				{
					if ( targetClient != null )
					{
						Log.Error( "There are multiple targets with this name, be more specific" );
						return;
					}
					else
					{
						targetClient = client;
					}
				}
			}

			if ( targetClient != null )
			{
				Log.Info( $"{caller.Name} forced a data load on {targetClient.Name}" );
				Instance.LoadSave( targetClient );
			}
			else
			{
				Log.Error( "No target found" );
			}
		}
	}

	[ConCmd.Server("ph_bringplayer")]
	public static void AdminBringPlayer( string target = "" )
	{
		var caller = ConsoleSystem.Caller;

		if ( caller == null )
			return;

		if ( string.IsNullOrEmpty( target ) )
		{
			return;
		}
		else
		{
			LobbyPawn targetPlayer = null;

			foreach ( var client in Client.All )
			{
				if ( client.Name.ToLower().Contains( target ) )
				{
					if ( targetPlayer != null )
					{
						Log.Error( "There are multiple targets with this name, be more specific" );
						return;
					}
					else
					{
						targetPlayer = client.Pawn as LobbyPawn;
					}
				}
			}

			if ( targetPlayer != null )
			{
				if( targetPlayer.LifeState == LifeState.Dead)
				{
					Log.Error( "that player is dead" );
					return;
				}

				var tr = Trace.Ray( caller.Pawn.EyePosition, caller.Pawn.EyePosition + caller.Pawn.EyeRotation.Forward * 999 )
					.WorldAndEntities()
					.Ignore( caller.Pawn )
					.Run();

				using (Prediction.Off())
					targetPlayer.Position = tr.EndPosition;

				Log.Info( $"{caller.Name} brought {targetPlayer.Client.Name} to them" );
			}
			else
			{
				Log.Error( "No target found" );
			}
		}
	}
}
