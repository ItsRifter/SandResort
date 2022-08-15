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

	public HeaderPanel ShopTitle;

	public ItemInfo itemInfo1;
	Panel ShopItemInfo;


	public ShopUI() {
		StyleSheet.Load( "UI/Styles/Lobby/ShopUi.scss" );
		StyleSheet.Load( "UI/Styles/Lobby/CSUIContruct.scss" );

		ShopRootPanel = Add.Panel( "ShopRootPanel" );

		Panel MainShop = ShopRootPanel.Add.Panel( "MainShop" );
		ShopTitle = new HeaderPanel( "Shop" );
		MainShop.AddChild( ShopTitle );
		shopItems = MainShop.Add.Panel( "shopItems" );

		ShopItemInfo = ShopRootPanel.Add.Panel( "itemShopInfo" );
		ShopItemInfo.AddChild( new HeaderPanel( "No Preview" ) );
		ShopItemInfo.Add.Label( "Click on an item to preview the model.", "text nopreviewAlert" );

		OpenShop();

		ShopRootPanel.AddChild( new CloseButton( () =>
		 {
			 CloseShop();
		 } ) );
	}

	public string title {
		get { return ShopTitle.Title; }
		set { ShopTitle.Title = value; }
	}

	public void OpenShop()
	{
		foreach ( var prop in PropBase.GetProps() )
		{
			ShopItem item = new ShopItem(() =>
			{
				ShopItemInfo.DeleteChildren();
				ShopItemInfo.AddChild( new HeaderPanel( "Preview" ) );
				ShopItemInfo.Add.Label( prop.PropName, "text Itemname" );
				itemInfo1 = new ItemInfo( prop.ModelPath );

				ShopItemInfo.AddChild( itemInfo1 );
			} ,() =>
			{
				ConsoleSystem.Run( "sc_buyitem", prop.ClassName );
				CloseShop();
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
