using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;

public partial class PHGame
{
	[ConCmd.Server("ph_buy_item")]
	public static void PurchaseItem(string item, int plyID)
	{
		Host.AssertServer();

		var buyer = ConsoleSystem.Caller.Pawn as PHPawn;

		if ( buyer == null )
			return;

		if ( buyer.Client.NetworkIdent != plyID )
			return;

		var boughtItem = TypeLibrary.Create<PHSuiteProps>( item );

		if( boughtItem == null )
		{
			Log.Error( $"Something went wrong purchasing -{item}-" );
			return;
		}

		if ( buyer.PlayCoins < boughtItem.SuiteItemCost)
		{
			boughtItem.Delete();
			buyer.PlaySound( "player_use_fail" );
			return;
		}

		if ( !buyer.PHInventory.CanAdd( boughtItem ) )
		{
			boughtItem.Delete();
			buyer.PlaySound( "player_use_fail" );
			return;
		}
		
		buyer.TakeCoins( boughtItem.SuiteItemCost );

		buyer.PlaySound("buy_item");

		buyer.PHInventory.AddItem( boughtItem );
	}

	[ConCmd.Server("ph_drag_item")]
	public static void SetItem(string prop)
	{
		var dragger = ConsoleSystem.Caller.Pawn as PHPawn;

		if ( dragger == null )
			return;

		if ( string.IsNullOrEmpty( prop ) )
			return;

		dragger.previewProp = TypeLibrary.Create<PHSuiteProps>(prop);
		dragger.previewProp.IsPreview = true;
		dragger.previewProp.Owner = dragger;
		dragger.previewProp.Spawn();
	}
}
