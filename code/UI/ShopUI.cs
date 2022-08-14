using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using SandCasle.UI;

public class ShopUI : Panel
{
    public Panel ShopRootPanel;
    public TimeSince lastOpen = 0;
    public Panel shopItems;

	public ItemInfo itemInfo1;
	Panel ShopItemInfo;


	public ShopUI() { 
		StyleSheet.Load( "UI/Styles/Lobby/ShopUi.scss" );
		StyleSheet.Load( "UI/Styles/Lobby/CSUIContruct.scss" );

        ShopRootPanel = Add.Panel("ShopRootPanel");

        Panel MainShop = ShopRootPanel.Add.Panel("MainShop");
        MainShop.AddChild(new HeaderPanel("Shop"));
        shopItems = MainShop.Add.Panel("shopItems");

        ShopItemInfo = ShopRootPanel.Add.Panel("itemShopInfo");
		ShopItemInfo.AddChild( new HeaderPanel( "No Preview" ) );

		OpenShop();

		ShopRootPanel.AddChild( new CloseButton( () =>
		 {
			 CloseShop();
		 } ) );
	}

	public void OpenShop()
	{
		foreach ( var prop in PropBase.GetProps() )
		{
			ShopItem item = new ShopItem(() =>
			{
				ShopItemInfo.DeleteChildren();
				ShopItemInfo.AddChild( new HeaderPanel( "Preview" ) );
				itemInfo1 = new ItemInfo( "models/citizen/citizen.vmdl" );

				ShopItemInfo.AddChild( itemInfo1 );
			} ,() =>
			{
				ConsoleSystem.Run( "sc_buyitem", prop.ClassName );
			} );
			item.Name = prop.PropName;
			item.Description = prop.Desc;
			item.Price = prop.Cost;
			item.iconImage = prop.IconImage;

			shopItems.AddChild( item );
		}
	}
	public void CloseShop() {
		Delete();
	}

	public override void Tick()
	{
		base.Tick();
	}
}
