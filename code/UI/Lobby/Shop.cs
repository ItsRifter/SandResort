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

	public void OpenShop( PHPawn player, Type shopType )
	{
		if ( hasOpened )
			return;

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
			var grabbingItem = PHGame.Instance.GrabSuiteItem( buyableItem );
			
			if ( grabbingItem == null )
				continue;

			var item = grabbingItem.First();


			if ( shopType.FullName == "BarShop" && item.Item4 != PHSuiteProps.ShopType.Bar )
				continue;
			else if ( shopType.FullName == "FurnitureShop" && item.Item4 != PHSuiteProps.ShopType.Furniture )
				continue;
			else if ( shopType.FullName == "ElectricShop" && item.Item4 != PHSuiteProps.ShopType.Electric )
				continue;

			Panel itemPnl = ShopItems.Add.Panel( "shop-item" );
			
			Panel info = itemPnl.Add.Panel( "shop-info" );
			info.Add.Label( $"{item.Item1} - ${item.Item3}", "shop-item-title" );
			info.Add.Label( item.Item2, "shop-item-description" );
			
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
	}

	public void PurchaseItem(string itemToBuy)
	{
		ConsoleSystem.Run( "ph_buy_item", itemToBuy, Local.Client.NetworkIdent );
	}

	public override void Tick()
	{
		base.Tick();

		var player = Local.Pawn as PHPawn;

		if ( player == null )
			return;

		if ( player.OpenShop )
			OpenShop(player, player.ShopKeeper.GetType());
		else
			CloseShop();

		SetClass( "openshop", player.OpenShop );
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

