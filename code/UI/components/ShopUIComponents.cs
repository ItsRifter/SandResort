using System;
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

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
	
	public class CondoItem : Panel
	{

	}
	public class ItemInfo : Panel
	{
		public Panel RootPanel;
		public Panel ControlsCamMode;
		public Panel modeAuto;
		public Panel modeDrag;

		public string Mode = "auto";

		public float cam_yaw = 0;
		public ScenePanel ShopItemScenePanel;
		public SceneWorld shopWorld;

		Angles CamAngles = new( 25.0f, 0.0f, 0.0f );
		Angles PreviusCamAngles = new( 25.0f, 0.0f, 0.0f );
		float CamDistance = 120;
		float targetDistance = 120;
		Vector3 CamPos => Vector3.Up * 10 + CamAngles.Direction * -CamDistance;

		public SceneModel itemPreview;
		public ItemInfo(string itemPreviewPath = "models/citizen_props/roadcone01.vmdl", string env = "testfloor" )
		{
			SetClass( "dragable", false );
			shopWorld = new SceneWorld();
			switch ( env )
			{
				case "testfloor":
					ShopItemScenePanel = Add.ScenePanel( shopWorld, CamPos, Rotation.From( CamAngles ), 70 );
					ShopItemScenePanel.Style.Width = Length.Percent( 100 );
					ShopItemScenePanel.Style.Height = Length.Percent( 100 );
					new SceneModel( shopWorld, "models/room.vmdl", Transform.Zero );
					new SceneLight( shopWorld, Vector3.Up * 60 + Vector3.Right * 60 + Vector3.Backward * 80, 400f, Color.White * 5f );
					new SceneLight( shopWorld, Vector3.Up * 80 + Vector3.Left * 30 + Vector3.Forward * 80, 400f, Color.White * 5f );
				break;
			}
			try
			{
				itemPreview = new SceneModel( shopWorld, itemPreviewPath, Transform.Zero );
			}
			catch (Exception ex)
			{
				itemPreview = new SceneModel( shopWorld, "models/citizen_props/roadcone01.vmdl", Transform.Zero );
				Log.Error( $"Blank string: {ex}" );
			}

			AddChild( ShopItemScenePanel );

			RootPanel = Add.Panel( "rootPanel" );
			ControlsCamMode = RootPanel.Add.Panel( "controls" );
			//modeAuto = ControlsCamMode.Add.Button( "", "modeBtn active", () =>
			//{
			//	foreach (var btn in ControlsCamMode.Children)
			//	{
			//		 btn.SetClass( "active", false );
			//	}
			//	modeAuto.SetClass( "active", true );
			//	Mode = "auto";
			//} );
			//modeDrag = ControlsCamMode.Add.Button( "", "modeBtn", () =>
			//{
			//	foreach ( var btn in ControlsCamMode.Children )
			//	{
			//		btn.SetClass( "active", false );
			//	}
			//	modeDrag.SetClass( "active", true );
			//	Mode = "drag";
			//} );

			modeAuto = ControlsCamMode.Add.Panel( "modeBtn active" );
			modeAuto.AddEventListener( "onClick", () =>
			{
				foreach ( var btn in ControlsCamMode.Children )
				{
					btn.SetClass( "active", false );
				}
				modeAuto.SetClass( "active", true );
				Mode = "auto";
				SetClass( "dragable", false );
			} );

			modeDrag = ControlsCamMode.Add.Panel( "modeBtn" );
			modeDrag.AddEventListener( "onClick", () =>
			{
				foreach ( var btn in ControlsCamMode.Children )
				{
					btn.SetClass( "active", false );
				}
				modeDrag.SetClass( "active", true );
				Mode = "drag";
				SetClass( "dragable", true );
			} );

			modeAuto.Add.Image( "ui/icons/icon-orbit.png" );
			modeDrag.Add.Image( "ui/icons/icon-drag.png" );

			//textItemName = RootPanel.Add.Label( "No item selected!", "itemname" );
			//textDescription = RootPanel.Add.Label( "", "Description" );
			//textPrice = RootPanel.Add.Label( "0$", "TextPrice" );

			//RootPanel.Add.Label( "hello world!", "text" );
			//var world = itemScenePreview.CreateSc
			//itemScenePreview = Add.ScenePanel( itemScenePreview, camAngle)
		}

		public override void OnMouseWheel( float value )
		{
			targetDistance += value * 10f;

			if ( targetDistance < 10 ) targetDistance = 10;
			if ( targetDistance > 300 ) targetDistance = 300;

			//CamDistance += value * 10f;
			CamDistance = CamDistance.Clamp( 10, 300 );

			base.OnMouseWheel( value );
		}
		public override void OnButtonEvent( ButtonEvent e )
		{
			if ( e.Button == "mouseleft")
			{
				if ( Mode == "drag" )
				{
					SetMouseCapture( e.Pressed );
				}
			}

			base.OnButtonEvent( e );
		}

		public override void Tick()
		{
			base.Tick();
			switch (Mode)
			{
				case "auto":
					//CamDistance = 120;
					cam_yaw++;
					if ( cam_yaw >= 360*4)
					{
						cam_yaw = 0;
					}
					CamAngles.pitch = 10;
					CamAngles.yaw = cam_yaw / 4;
					CamAngles.pitch.Clamp( 0, 90 );
					CamDistance = CamDistance.LerpTo(120,Time.Delta * 5f, true);
					CamDistance.Clamp( 90, 100 );
					ShopItemScenePanel.CameraPosition = CamPos;
					//itemPreview.Bounds.Center
					ShopItemScenePanel.CameraRotation =  Rotation.From( CamAngles );
					break;
				case "drag":
					if (HasMouseCapture)
					{
						CamAngles.pitch += Mouse.Delta.y;
						CamAngles.yaw -= Mouse.Delta.x;
						CamAngles.pitch = CamAngles.pitch.Clamp( -12, 90 );
					}

					PreviusCamAngles = CamAngles;

					ShopItemScenePanel.CameraPosition = CamPos;
					ShopItemScenePanel.CameraRotation = Rotation.From( CamAngles );

					CamDistance = CamDistance.LerpTo(targetDistance,Time.Delta * 5f, true);
					break;
			}



		}

		//public string NameInfo
		//{
		//	get { return textItemName.Text; }
		//	set { textItemName.Text = value; }
		//}
		//public string DescriptionInfo
		//{
		//	get { return textDescription.Text; }
		//	set { textDescription.Text = value; }
		//}
		//public int PriceInfo
		//{
		//	get { return int.Parse( textPrice.Text ); }
		//	set { textPrice.Text = value.ToString() + "$"; }
		//}
		//	public int ItemIcon
		//	{
		//		get { return int.Parse( textDescription.Text); }
		//		set { textDescription.Text = value.ToString(); }
		//	}
	}

	public class CloseButton : Panel
	{
		public Button CloseBTN;
		public CloseButton( Action OnCloseClick )
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
		public HeaderPanel( string title, bool haveMarginTop = true )
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
