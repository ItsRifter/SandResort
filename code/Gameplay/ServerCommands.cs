using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;

public partial class PHGame
{
	[ConCmd.Server("ph_test")]
	public static void PurchaseItem(string item, int plyID)
	{
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
			return;
		}

		buyer.TakeCoins( boughtItem.SuiteItemCost );

		buyer.previewProp = boughtItem;
		buyer.previewProp.IsPreview = true;
		buyer.previewProp.Spawn();
	}
}
