﻿using System;
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

		boughtItem.Delete();
	}

	[ConCmd.Server( "ph_drag_item" )]
	public static void DragItem( string dragName, string dragClass )
	{
		var dragger = ConsoleSystem.Caller.Pawn as PHPawn;

		if ( dragger == null )
			return;

		dragger.PreviewProp = TypeLibrary.Create<PHSuiteProps>( dragName );
		dragger.PreviewProp.Name = dragClass;
		dragger.PreviewProp.Owner = dragger;
		dragger.PreviewProp.IsPreview = true;
		dragger.PreviewProp.IsMovingFrom = true;
		dragger.PreviewProp.Spawn();

		dragger.timeToWaitPlacing = 0;
	}

	[ConCmd.Server( "ph_select_item" )]
	public static void SetItem(string prop)
	{
		var setter = ConsoleSystem.Caller.Pawn as PHPawn;

		if ( setter == null )
			return;

		if( setter.PreviewProp != null )
		{
			setter.PreviewProp.Delete();
			setter.PreviewProp = null;
		}

		setter.PreviewProp = TypeLibrary.Create<PHSuiteProps>(prop);
		setter.PreviewProp.IsPreview = true;
		setter.PreviewProp.Owner = setter;
		setter.PreviewProp.Spawn();
	}

	[ConCmd.Server("ph_qmenu_clear")]
	public static void CleanUpQMenu()
	{
		var player = ConsoleSystem.Caller.Pawn as PHPawn;

		if ( player == null )
			return;

		if ( player.PreviewProp == null )
			return;

		player.PreviewProp.Delete();
		player.PreviewProp = null;
	}

	[ConCmd.Server( "ph_qmenu_reset" )]
	public static void ResetQMenu()
	{
		var player = ConsoleSystem.Caller.Pawn as PHPawn;

		if ( player == null )
			return;

		player.PHInventory.ClientInventory.Clear();

	}
}
