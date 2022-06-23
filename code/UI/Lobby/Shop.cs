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

	bool hasOpened = false;
	public Shop()
	{
		StyleSheet.Load( "UI/Styles/Lobby/Shop.scss" );
		SetTemplate( "UI/html/Lobby/Shop.html" );

		ShopMenu = Add.Panel( "main-shop" );

		Panel shop = ShopMenu.Add.Panel( "shop" );
		ShopName_Container = shop.Add.Panel( "shop-title" );
		ShopName = ShopName_Container.Add.Label( "???", "shop-title" );

		ShopItems = shop.Add.Panel( "shop-items" );
	}

	public void OpenShop( LobbyPawn player, Type shopType )
	{
		if ( hasOpened )
			return;

		Style.ZIndex = 5;

		hasOpened = true;

		if ( shopType.FullName == "AdminNPC" && !PHGame.Instance.AdminList.Contains( player.Client.PlayerId ) )
		{
			CloseShop();
			return;
		}

		var tempNPC = TypeLibrary.Create<ShopKeeperBase>( shopType.FullName );

		ShopName.SetText( tempNPC.NPCName );

		tempNPC.Delete();

		foreach ( var buyableItem in PHGame.Instance.GetAllSuiteProps() )
		{
			var item = PHGame.Instance.GrabSuiteItem( buyableItem );
			
			if ( item == null )
				continue;

			if ( shopType.FullName == "BarShop" && item.ShopSeller != PHSuiteProps.ShopType.Bar )
				continue;
			else if ( shopType.FullName == "FurnitureShop" && item.ShopSeller != PHSuiteProps.ShopType.Furniture )
				continue;
			else if ( shopType.FullName == "ElectricShop" && item.ShopSeller != PHSuiteProps.ShopType.Electric )
				continue;

			Panel itemPnl = ShopItems.Add.Panel( "shop-item" );

			Panel itemImg = itemPnl.Add.Panel("shop-item-img");

			itemImg.Style.SetBackgroundImage( item.SuiteItemImage );

			Panel info = itemPnl.Add.Panel( "shop-info" );
			info.Add.Label( $"{item.SuiteItemName} - {item.SuiteItemCost:C0}", "shop-item-title" );
			info.Add.Label( item.SuiteItemDesc, "shop-item-description" );

			item.Delete();

			itemPnl.AddEventListener( "onclick", () =>
			{
				PurchaseItem(buyableItem);
			} );
		}
	}

	public void CloseShop()
	{
		hasOpened = false;
		ShopItems.DeleteChildren();
		Style.ZIndex = 0;
	}

	public void PurchaseItem(string itemToBuy)
	{
		ConsoleSystem.Run( "ph_buy_item", itemToBuy, Local.Client.NetworkIdent );
	}

	public override void Tick()
	{
		base.Tick();

		if ( Local.Pawn is not LobbyPawn player )
			return;

		if ( player.InteractNPC == null )
		{
			hasOpened = false;
			CloseShop();
		}

		if ( player.InteractNPC is ShopKeeperBase && !hasOpened && Input.Pressed( InputButton.Use ) )
			OpenShop(player, player.InteractNPC.GetType());
		else if (Input.Pressed(InputButton.Use) && hasOpened )
			CloseShop();

		SetClass( "openshop", hasOpened );
	}
}


internal class ShopTagComponent : EntityComponent<ShopKeeperBase>
{
	ShopTag shopTag;

	protected override void OnActivate()
	{
		shopTag = new ShopTag( Entity?.NPCName );
	}

	protected override void OnDeactivate()
	{
		shopTag?.Delete();
		shopTag = null;
	}

	[Event.Frame]
	public void FrameUpdate()
	{
		var tx = Entity.GetAttachment( "hat" ) ?? Entity.Transform;
		tx.Position += Vector3.Up * 5.0f;
		tx.Rotation = Rotation.LookAt( -CurrentView.Rotation.Forward );

		shopTag.Transform = tx;
	}

	[Event.Frame]
	public static void SystemUpdate()
	{
		foreach ( var shopKeeper in Sandbox.Entity.All.OfType<ShopKeeperBase>() )
		{
			var player = Local.Pawn;

			var shouldRemove = player.Position.Distance( shopKeeper.Position ) > 250;
			shouldRemove = shouldRemove || player.IsDormant;

			if ( shouldRemove )
			{
				var c = player.Components.Get<ShopTagComponent>();
				c?.Remove();
				continue;
			}

			shopKeeper.Components.GetOrCreate<ShopTagComponent>();
		}
	}
}

public class ShopTag : WorldPanel
{
	public Label NameLabel;

	internal ShopTag( string title )
	{
		StyleSheet.Load( "UI/Styles/Lobby/ShopTag.scss" );

		NameLabel = Add.Label( title, "title" );

		// this is the actual size and shape of the world panel
		PanelBounds = new Rect( -500, -100, 1000, 200 );
	}
}

