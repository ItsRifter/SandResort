using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

public partial class Shop : Panel
{
	public Panel ShopMenu;

	public Panel ShopName_Container;
	public Label ShopName;
	public Panel ShopItems;

	bool isOpen;
	TimeSince lastOpened;

	public Shop()
	{
		StyleSheet.Load( "UI/Styles/Lobby/Shop.scss" );
		SetTemplate( "UI/html/Lobby/Shop.html" );

		ShopMenu = Add.Panel( "main-shop" );

		Panel shop = ShopMenu.Add.Panel( "shop" );
		ShopName_Container = shop.Add.Panel( "shop-title" );
		ShopName = ShopName_Container.Add.Label( "Test Shop", "shop-title" );

		ShopItems = shop.Add.Panel( "shop-items" );


		/*Panel item = ShopItems.Add.Panel( "shop-item" );

		Panel info = item.Add.Panel( "shop-info" );
		info.Add.Label( "Beer Barrel - 500", "shop-item-title" );
		info.Add.Label( "A barrel of beer, don't get too drunk", "shop-item-description" );

		item.AddEventListener( "onclick", () =>
		{
			PurchaseItem();
		} );
*/
	}

	public void OpenShop()
	{
		foreach ( var buyableItem in PHGame.Instance.GetAllSuiteProps() )
		{
			var itemDisplay = TypeLibrary.Create<PHSuiteProps>(buyableItem);

			Panel item = ShopItems.Add.Panel( "shop-item" );
			
			Panel info = item.Add.Panel( "shop-info" );
			info.Add.Label( $"{itemDisplay.SuiteItemName} - {itemDisplay.SuiteItemCost}", "shop-item-title" );
			info.Add.Label( itemDisplay.SuiteItemDesc, "shop-item-description" );
			
			item.AddEventListener( "onclick", () =>
			{
				PurchaseItem(buyableItem);
			} );

			itemDisplay.Delete();
		}
	}

	public void CloseShop()
	{
		ShopItems.DeleteChildren();
	}

	public Panel CreateItem(string title, string description, Action onClick)
    {
		Panel item = Add.Panel("shop-item");

		Panel info = item.Add.Panel("shop-info");
		info.Add.Label(title, "shop-item-title");
		info.Add.Label(description, "shop-item-description");

		item.AddEventListener("onclick", () =>
		{
			onClick();
		});

		return item;
	}

	public void PurchaseItem(string itemToBuy)
	{
		ConsoleSystem.Run( "ph_buy_item", itemToBuy, Local.Client.NetworkIdent );
		//PHGame.PurchaseItem("beerbarrel");
	}

	public override void Tick()
	{
		base.Tick();

		var player = Local.Pawn as PHPawn;

		if ( player == null )
			return;

		var tr = Trace.Ray( player.EyePosition, player.EyePosition + player.EyeRotation.Forward * 50)
			.Ignore( player )
			.Run();

		if ( tr.Entity is null )
		{
			isOpen = false;
			CloseShop();
		}

		if (Input.Pressed(InputButton.Use) && lastOpened > 0.3f && tr.Entity is AdminNPC)
		{
			isOpen = !isOpen;
			lastOpened = 0;

			if ( isOpen )
				OpenShop();
			else
				CloseShop();
		}

		SetClass( "openshop", isOpen );
	}


}

