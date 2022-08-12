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
            item.Name = "Item number: " + (i+1);
            item.Description = "This is a test";
            item.Price = 250;
            item.iconImage = "ui/alex.jpg";

            shopItems.AddChild(item);
        }

        Panel ShopItemInfo = ShopRootPanel.Add.Panel("itemShopInfo");
		ShopItemInfo.AddChild( new HeaderPanel( "Item Info" ) );
		ShopItemInfo.AddChild( new ItemInfo() );
		//ShopItemInfo.Add.Label("// TODO Make Render Screne and info", "text itemName");

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
			Log.Info( $"shop menu open: {isOpen}" );
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

    public ShopItem(Action onClick = null, Action onBuyClick = null)
    {
        ShopItemPanel = Add.Panel("ShopItemPanel");
        itemIcon = ShopItemPanel.Add.Panel("itemImg");
        Panel itemInfo = ShopItemPanel.Add.Panel("itemInfo");
        itemName = itemInfo.Add.Label("Missing Name", "itemName");
        itemDescription = itemInfo.Add.Label("Missing Description", "itemDescription");
        itemPrice = itemInfo.Add.Label("Free", "itemPrice");
		Panel Buttons = Add.Panel("buttons");
		//Button moreinfoButton = Buttons.Add.Button( "More info", "normalButton", () => {
		//	Log.Info( "Item Pressed" );
		//	if ( onClick != null )
		//	{
		//		Log.Info( "Item Action!" );
		//		onClick();
		//	}
		//} );
		Button buyButton = Buttons.Add.Button("Buy", "buyButton", () => {
            Log.Info("Buy Button Pressed");
            if (onBuyClick != null)
            {
                Log.Info("Buy Action!");
                onBuyClick();
            }
        });

		ShopItemPanel.AddEventListener("onClick", () => {
            Log.Info("Shop Item Clicked");
            if (onClick != null)
            {
                Log.Info("Item Action!");
                onClick();
            }
        });

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

public class ItemInfo : Panel
{
	public Panel RootPanel;
	public Label textItemName;

	//readonly ScenePanel itemScenePreview;
	//Angles CamAngles = new( 25.0f, 0f, 0f );
	//float CamDistance = 120;
	//Vector3 CamPos => Vector3.Up * 10 + CamAngles.Direction * -CamDistance;
	public ItemInfo()
	{
		RootPanel = Add.Panel( "rootPanel" );
		textItemName = RootPanel.Add.Label( "No item name", "itemname" );
		//RootPanel.Add.Label( "hello world!", "text" );
		//var world = itemScenePreview.CreateSc
		//itemScenePreview = Add.ScenePanel( itemScenePreview, camAngle)
	}

	public string ItemName
	{
		get { return textItemName.Text; }
		set { textItemName.Text = value; }
	}
}
