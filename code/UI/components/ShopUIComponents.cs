using System;
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using SandCasle.UI;

namespace SandCasle.UI
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

			Button buyButton = Buttons.Add.Button( "Buy", "buyButton", () =>
			{
				if ( onBuyClick != null )
				{
					onBuyClick();
				}
			} );

			ShopItemPanel.AddEventListener( "onClick", () =>
			{
				if ( onClick != null )
				{
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

	public class closeButton : Panel
	{
		public Button CloseBTN;
		public closeButton( Action OnCloseClick )
		{
			CloseBTN = Add.Button( "Close", "btn" );
			CloseBTN.AddEventListener( "onClick", () =>
			{
				OnCloseClick();
			} );
		}
	}

	public class HeaderPanel : Panel
	{
		public Label textLabel;
		public HeaderPanel( String title, bool haveMarginTop = true )
		{
			Panel MainHeaderPanel = Add.Panel( "HeaderPanel" );
			textLabel = MainHeaderPanel.Add.Label( title, "headerTitle" );
			MainHeaderPanel.Add.Panel( "headerSeparator" );

			if ( haveMarginTop )
			{
				AddClass( "addMarginTop" );
			}
		}
		public string Title
		{
			get { return textLabel.Text; }
			set { textLabel.Text = value; }
		}
	}
	public class ClientSuitePanel : Panel
	{
		public Label SuiteTitle;
		public Panel SuiteBackground;
		public Label suiteName;
		public Button CheckoutBtn;
		public ClientSuitePanel()
		{
			Panel RootPanel = Add.Panel( "ClientSuitePanel" );
			SuiteBackground = RootPanel.Add.Panel( "SuiteBackground" );
			RootPanel.AddChild( new HeaderPanel( "Your Condo." ) );
			Panel SuiteInfo = RootPanel.Add.Panel( "SuiteInfo" );
			Panel SuiteInfoImage = SuiteInfo.Add.Panel( "SuiteInfoImage" );

			Panel SuiteInfoStatus = SuiteInfo.Add.Panel( "SuiteInfoStatus" );
			suiteName = SuiteInfoStatus.Add.Label( "No Name", "suiteText SuiteTitle" );
			CheckoutBtn = SuiteInfoStatus.Add.Button( "Check out", "checkoutBtn" );
		}
	}
}
