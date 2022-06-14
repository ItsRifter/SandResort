using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;

public partial class PHGame
{

	public void PurchaseItem(string item)
	{
		var buyer = Local.Pawn as PHPawn;

		if ( buyer == null )
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
