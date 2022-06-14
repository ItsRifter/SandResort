using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;
public partial class PCGame
{
	//Hardcoded for now until we can figure out an websocket admin way
	public static string[] AdminList { get; protected set; }

	static float eyeDist = 150.0f;

	[ConCmd.Server( "pc_coins_give" )]
	public static void AdminGiveCoins( int amount, string target = "" )
	{
		if ( AdminList == null || !AdminList.Contains( ConsoleSystem.Caller.Name ) )
		{
			Log.Error( "You do not have access to this command" );
			return;
		}

		if ( amount <= 0 || amount >= 10000000 )
		{
			Log.Error( "Invalid amount" );
			return;
		}

		var player = ConsoleSystem.Caller.Pawn as PCPawn;

		if ( player == null )
			return;

		if ( string.IsNullOrEmpty( target ) )
		{
			Log.Info( $"{player.Client.Name} gave themself {amount} PlayCoins" );
			player.GiveCoins( amount );
		}
		else
		{
			PCPawn targetPlayer = null;

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
						targetPlayer = client.Pawn as PCPawn;
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

	[ConCmd.Server( "pc_coins_take" )]
	public static void AdminTakeCoins( int amount, string target = "" )
	{
		if ( amount <= 0 || amount >= 10000000 )
		{
			Log.Error( "Invalid amount" );
			return;
		}

		if ( AdminList == null || !AdminList.Contains( ConsoleSystem.Caller.Name ) )
		{
			Log.Error( "You do not have access to this command" );
			return;
		}

		var player = ConsoleSystem.Caller.Pawn as PCPawn;

		if ( player == null )
			return;

		if ( string.IsNullOrEmpty( target ) )
		{
			Log.Info( $"{player.Client.Name} removed {amount} PlayCoins from themselves" );
			player.TakeCoins( amount );
		}
		else
		{
			PCPawn targetPlayer = null;

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
						targetPlayer = client.Pawn as PCPawn;
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

	[ConCmd.Server( "pc_spawn_item_suite" )]
	public static void AdminSpawnItem( string itemSuiteName )
	{
		if ( AdminList == null || !AdminList.Contains( ConsoleSystem.Caller.Name ) )
		{
			Log.Error( "You do not have access to this command" );
			return;
		}

		if ( TypeLibrary.GetTypeByName<PCSuiteProps>( itemSuiteName ) == null )
			return;

		var player = ConsoleSystem.Caller.Pawn as PCPawn;

		if ( player == null )
			return;

		var tr = Trace.Ray( player.EyePosition, player.EyePosition + player.EyeRotation.Forward * eyeDist )
			.Ignore( player )
			.Run();

		var itemSuite = TypeLibrary.Create<PCSuiteProps>( itemSuiteName );

		itemSuite.Rotation = player.Rotation.Inverse;
		itemSuite.Position = tr.EndPosition;
		itemSuite.Spawn();
	}

	[ConCmd.Server( "pc_spawn_npc" )]
	public static void AdminSpawnNPC( string npcName )
	{
		if ( AdminList == null || !AdminList.Contains( ConsoleSystem.Caller.Name ) )
		{
			Log.Error( "You do not have access to this command" );
			return;
		}

		if ( TypeLibrary.GetTypeByName<PCBaseNPC>( npcName ) == null )
			return;

		var player = ConsoleSystem.Caller.Pawn as PCPawn;

		if ( player == null )
			return;

		var tr = Trace.Ray( player.EyePosition, player.EyePosition + player.EyeRotation.Forward * eyeDist )
			.Ignore( player )
			.Run();

		var npc = TypeLibrary.Create<PCBaseNPC>( npcName );

		npc.Rotation = player.Rotation.Inverse;
		npc.Position = tr.EndPosition;
		npc.Spawn();
	}
}
