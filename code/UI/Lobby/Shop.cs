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

	bool isOpen;
	TimeSince lastOpened;

	public Shop()
	{
		StyleSheet.Load( "UI/Styles/Lobby/Shop.scss" );
		SetTemplate( "UI/html/Lobby/Shop.html" );

		ShopMenu = Add.Panel( "main-shop" );

		Panel shop = ShopMenu.Add.Panel( "shop" );
		ShopName_Container = shop.Add.Panel( "shop-title" );
		ShopName = ShopName_Container.Add.Label( "Bar", "shop-title" );

		Panel ShopItems = shop.Add.Panel( "shop-items" );

		//ShopItems.AddChild( AddShopItem( "lol", "lol2" ) );
		//ShopItems.Add.AddShopItem( "lol", "lol2" );
		Panel item = ShopItems.Add.Panel( "shop-item" );

		Panel info = item.Add.Panel( "shop-info" );
		info.Add.Label( "Beer Barrel - 500", "shop-item-title" );
		info.Add.Label( "A barrel of beer, don't get too drunk", "shop-item-description" );

		item.AddEventListener( "onclick", () =>
		{
			PurchaseItem();
		} );

	}

	public void PurchaseItem()
	{
		Log.Info( "yay" );
		PHGame.Instance.PurchaseItem("beerbarrel");
	}

	public Panel AddShopItem(string title, string description)
	{
		Panel item = Add.Panel("shop-item");

		Panel info = item.Add.Panel("shop-info");
		info.Add.Label( title , "shop-item-title");
		info.Add.Label( description, "shop-item-description");

		return item;
	}

	public override void Tick()
	{
		base.Tick();

		if(Input.Pressed(InputButton.Menu) && lastOpened > 0.3f)
		{
			isOpen = !isOpen;
			lastOpened = 0;
		}

		SetClass( "openshop", isOpen );
	}


}

