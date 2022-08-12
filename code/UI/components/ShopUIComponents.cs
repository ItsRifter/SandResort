using System;
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

namespace SC.UI.Construct
{
	public class ShopItem : Panel
	{
		public Label itemName;
		public Label itemDescription;
		public Label itemPrice;
		public Panel itemIcon;
		public Panel ShopItemPanel;

		public ShopItem( Action onClick = null, Action onBuyClick = null )
		{
			ShopItemPanel = Add.Panel( "ShopItemPanel" );
			itemIcon = ShopItemPanel.Add.Panel( "itemImg" );

			Panel itemInfo = ShopItemPanel.Add.Panel( "itemInfo" );

			itemName = itemInfo.Add.Label( "Missing Name", "itemName" );
			itemDescription = itemInfo.Add.Label( "Missing Description", "itemDescription" );
			itemPrice = itemInfo.Add.Label( "Free", "itemPrice" );

			Panel Buttons = Add.Panel( "buttons" );

			Button buyButton = Buttons.Add.Button( "Buy", "buyButton", () => {
				Log.Info( "Buy Button Pressed" );
				if ( onBuyClick != null )
				{
					Log.Info( "Buy Action!" );
					onBuyClick();
				}
			} );

			ShopItemPanel.AddEventListener( "onClick", () => {
				Log.Info( "Shop Item Clicked" );
				if ( onClick != null )
				{
					Log.Info( "Item Action!" );
					onClick();
				}
			} );

		}

		public string Name
		{
			get { return itemName.Text; }
			set { itemName.Text = value; }
		}
		public string Description
		{
			get { return itemDescription.Text; }
			set { itemDescription.Text = value; }
		}
		public int Price
		{
			get { return int.Parse( itemPrice.Text ); }
			set { itemPrice.Text = value.ToString() + "$"; }
		}
		public string iconImage
		{
			set { itemIcon.Style.SetBackgroundImage( value ); }
		}

	}

	public class ItemInfo : Panel
	{
		public Panel RootPanel;
		public Label textItemName;
		public Label textDescription;
		public Label textPrice;
		public Panel itemIcon;

		//readonly ScenePanel itemScenePreview;
		//Angles CamAngles = new( 25.0f, 0f, 0f );
		//float CamDistance = 120;
		//Vector3 CamPos => Vector3.Up * 10 + CamAngles.Direction * -CamDistance;
		public ItemInfo()
		{
			RootPanel = Add.Panel( "rootPanel" );
			textItemName = RootPanel.Add.Label( "No item selected!", "itemname" );
			textDescription = RootPanel.Add.Label( "", "Description" );
			textPrice = RootPanel.Add.Label( "0$", "TextPrice" );

			//RootPanel.Add.Label( "hello world!", "text" );
			//var world = itemScenePreview.CreateSc
			//itemScenePreview = Add.ScenePanel( itemScenePreview, camAngle)
		}

		public string NameInfo
		{
			get { return textItemName.Text; }
			set { textItemName.Text = value; }
		}
		public string DescriptionInfo
		{
			get { return textDescription.Text; }
			set { textDescription.Text = value; }
		}
		public int PriceInfo
		{
			get { return int.Parse( textPrice.Text ); }
			set { textPrice.Text = value.ToString() + "$"; }
		}
		//	public int ItemIcon
		//	{
		//		get { return int.Parse( textDescription.Text); }
		//		set { textDescription.Text = value.ToString(); }
		//	}
	}
}
