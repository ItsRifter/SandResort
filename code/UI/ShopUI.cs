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
    public ShopUI() { 
        SetClass("open", isOpen);
		StyleSheet.Load( "UI/Styles/Lobby/ShopUi.scss" );
		StyleSheet.Load( "UI/Styles/Lobby/CSUIContruct.scss" );

        ShopRootPanel = Add.Panel("ShopRootPanel");

        Panel MainShop = ShopRootPanel.Add.Panel("MainShop");
        MainShop.AddChild(new HeaderPanel("Shop"));
        shopItems = MainShop.Add.Panel("shopItems");

        for (int i = 0; i < 12; i++)
        {
            ShopItem item = new ShopItem();
            item.Name = "Item number: none";
            item.Description = "This is a test";
            item.Price = i;
            item.iconImage = "ui/alex.jpg";

            shopItems.AddChild(item);
        }

        // Panel ItemInfo = ShopRootPanel.Add.Panel("MainShop");
        // ItemInfo.AddChild(new HeaderPanel("Info"));

    }

    public override void Tick()
	{
		base.Tick();

		if(lastOpen > 0.1 && Input.Pressed(InputButton.Menu))
		{
			lastOpen = 0;
			isOpen = !isOpen;
			SetClass("open", isOpen);
			Log.Info( $"receiptionist menu open: {isOpen}" );
		}
	}
}

public class ShopItem : Panel
{
    public Label itemName;
    public Label itemDescription;
    public Label itemPrice;
    public Panel itemIcon;
    public Panel ShopItemPanel;

    public ShopItem()
    {
        ShopItemPanel = Add.Panel("ShopItemPanel");
        itemIcon = ShopItemPanel.Add.Panel("itemImg");
        Panel itemInfo = ShopItemPanel.Add.Panel("itemInfo");
        itemName = itemInfo.Add.Label("Missing Name", "itemName");
        itemDescription = itemInfo.Add.Label("Missing Description", "itemDescription");
        itemPrice = itemInfo.Add.Label("Free", "itemPrice");
    }
        
    public string Name { 
        get { return itemName.Text; }
        set { itemName.Text = value; }
    }
    public string Description { 
        get { return itemDescription.Text; }
        set { itemDescription.Text = value; }
    }
    public int Price {
        get { return int.Parse(itemPrice.Text); }
        set { itemPrice.Text = value.ToString() + "$"; }
    }
    public string iconImage  {
        set { itemIcon.Style.SetBackgroundImage(value); }
    }

}