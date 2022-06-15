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
	public IList<string> AdminList { get; protected set; }

	static float eyeDist = 150.0f;

	[ConCmd.Server( "ph_coins_give" )]
	public static void AdminGiveCoins( int amount, string target = "" )
	{
		if ( Instance.AdminList == null || !Instance.AdminList.Contains( ConsoleSystem.Caller.Name ) )
		{
			Log.Error( "You do not have access to this command" );
			return;
		}

		if ( amount <= 0 || amount >= 10000000 )
		{
			Log.Error( "Invalid amount" );
			return;
		}

		var player = ConsoleSystem.Caller.Pawn as PHPawn;

		if ( player == null )
			return;

		if ( string.IsNullOrEmpty( target ) )
		{
			Log.Info( $"{player.Client.Name} gave themself {amount} PlayCoins" );
			player.GiveCoins( amount );
		}
		else
		{
			PHPawn targetPlayer = null;

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
						targetPlayer = client.Pawn as PHPawn;
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

		if ( Instance.AdminList == null || !Instance.AdminList.Contains( ConsoleSystem.Caller.Name ) )
		{
			Log.Error( "You do not have access to this command" );
			return;
		}

		var player = ConsoleSystem.Caller.Pawn as PHPawn;

		if ( player == null )
			return;

		if ( string.IsNullOrEmpty( target ) )
		{
			Log.Info( $"{player.Client.Name} removed {amount} PlayCoins from themselves" );
			player.TakeCoins( amount );
		}
		else
		{
			PHPawn targetPlayer = null;

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
						targetPlayer = client.Pawn as PHPawn;
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
		if ( Instance.AdminList == null || !Instance.AdminList.Contains( ConsoleSystem.Caller.Name ) )
		{
			Log.Error( "You do not have access to this command" );
			return;
		}

		if ( TypeLibrary.GetTypeByName<PHSuiteProps>( itemSuiteName ) == null )
			return;

		var player = ConsoleSystem.Caller.Pawn as PHPawn;

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
		if ( Instance.AdminList == null || !Instance.AdminList.Contains( ConsoleSystem.Caller.Name ) )
		{
			Log.Error( "You do not have access to this command" );
			return;
		}

		if ( TypeLibrary.GetTypeByName<ShopKeeperBase>( npcName ) == null )
			return;

		var player = ConsoleSystem.Caller.Pawn as PHPawn;

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
	public static void AdminGiveItem( string itemName )
	{
		if ( !Instance.AdminList.Contains( ConsoleSystem.Caller.Name ) )
			return;
		
		var player = ConsoleSystem.Caller.Pawn as PHPawn;

		if ( player == null )
			return;

		if ( TypeLibrary.GetTypeByName<PHUsableItemBase>( itemName ) == null )
			return;

		var item = TypeLibrary.Create<PHUsableItemBase>( itemName );
		item.SetParent( player, true );

		player.PHInventory.AddCosmetic( item );

	}
}
