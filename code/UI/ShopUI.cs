using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using SC.UI.Construct;

public class ShopUI : Panel
{
    public bool isOpen = false;
    public Panel ShopRootPanel;
    public TimeSince lastOpen = 0;
    public Panel shopItems;

	public ItemInfo itemInfo1;


	public ShopUI() { 
        SetClass("open", isOpen);
		StyleSheet.Load( "UI/Styles/Lobby/ShopUi.scss" );
		StyleSheet.Load( "UI/Styles/Lobby/CSUIContruct.scss" );

        ShopRootPanel = Add.Panel("ShopRootPanel");

        Panel MainShop = ShopRootPanel.Add.Panel("MainShop");
        MainShop.AddChild(new HeaderPanel("Shop"));
        shopItems = MainShop.Add.Panel("shopItems");

        Panel ShopItemInfo = ShopRootPanel.Add.Panel("itemShopInfo");
		ShopItemInfo.AddChild( new HeaderPanel( "Item Info" ) );
		itemInfo1 = new ItemInfo();
		itemInfo1.DescriptionInfo = "Select an item for more info.";
		ShopItemInfo.AddChild( itemInfo1 );

	}

	public void OpenShop()
	{
		foreach ( var prop in PropBase.GetProps() )
		{
			ShopItem item = new ShopItem(() =>
			{
				itemInfo1.NameInfo = prop.PropName;
				itemInfo1.DescriptionInfo = prop.Desc;
				itemInfo1.PriceInfo = prop.Cost;
			} );
			item.Name = prop.PropName;
			item.Description = prop.Desc;
			item.Price = prop.Cost;
			item.iconImage = prop.IconImage;

			shopItems.AddChild( item );
		}
	}

	public void CloseShop()
	{
		shopItems.DeleteChildren();
		itemInfo1.NameInfo = "No item selected!";
		itemInfo1.DescriptionInfo= "Select an item for more info.";
		itemInfo1.PriceInfo = 0;
	}

	public override void Tick()
	{
		base.Tick();

		if ( lastOpen > 0.1 && Input.Pressed( InputButton.Menu ) )
		{
			lastOpen = 0;
			isOpen = !isOpen;
			SetClass( "open", isOpen );


			switch ( isOpen )
			{
				case true:
					OpenShop();
					break;
				case false:
					CloseShop();
					break;
			}

		}
	}
}
