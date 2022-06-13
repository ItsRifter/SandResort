using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;
public partial class PCGame
{
	[ConCmd.Admin("pc_coins_give")]
	public static void AdminGiveCoins(int amount, string target = "")
	{
		var player = ConsoleSystem.Caller.Pawn as PCPawn;

		if ( player == null )
			return;

		if( string.IsNullOrEmpty(target) )
		{
			Log.Info( $"{player.Client.Name} gave themself {amount} PlayCoins" );
			player.GiveCoins( amount );
		}
		else
		{
			PCPawn targetPlayer = null;
			
			foreach ( var client in Client.All )
			{
				if(client.Name.Contains(target) )
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

			if( targetPlayer != null)
			{
				Log.Info( $"{player.Client.Name} gave {targetPlayer.Client.Name} {amount} PlayCoins" );
				targetPlayer.GiveCoins( amount );
			} else
			{
				Log.Error( "No target found" );
			}
		}
	}

	[ConCmd.Admin( "pc_coins_take" )]
	public static void AdminTakeCoins( int amount, string target = "" )
	{
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
				if ( client.Name.Contains( target ) )
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
}

