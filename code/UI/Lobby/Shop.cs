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

	bool isOpen;
	TimeSince lastOpened;

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

		if ( shopType.FullName == "AdminNPC" && !PHGame.Instance.AdminList.Contains( player.Client.Name ) )
		{
			CloseShop();
			isOpen = false;
			return;
		}

		var tempNPC = TypeLibrary.Create<ShopKeeperBase>( shopType.FullName );

		ShopName.SetText( tempNPC.NPCName );

		tempNPC.Delete();

		foreach ( var buyableItem in PHGame.Instance.GetAllSuiteProps() )
		{
			var itemDisplay = TypeLibrary.Create<PHSuiteProps>(buyableItem);

			if ( shopType.FullName == "BarShop" && itemDisplay.ShopSeller != PHSuiteProps.ShopType.Bar )
			{
				itemDisplay.Delete();
				continue;
			}
			else if ( shopType.FullName == "FurnitureShop" && itemDisplay.ShopSeller != PHSuiteProps.ShopType.Furniture )
			{
				itemDisplay.Delete();
				continue;
			}
			else if ( shopType.FullName == "ElectricShop" && itemDisplay.ShopSeller != PHSuiteProps.ShopType.Electric )
			{
				itemDisplay.Delete();
				continue;
			}

			Panel item = ShopItems.Add.Panel( "shop-item" );
			
			Panel info = item.Add.Panel( "shop-info" );
			info.Add.Label( $"{itemDisplay.SuiteItemName} - ${itemDisplay.SuiteItemCost}", "shop-item-title" );
			info.Add.Label( itemDisplay.SuiteItemDesc, "shop-item-description" );
			
			item.AddEventListener( "onclick", () =>
			{
				PurchaseItem(buyableItem);
			} );

			itemDisplay.Delete();
		}
	}

	public void CloseShop()
	{
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

		var tr = Trace.Ray( player.EyePosition, player.EyePosition + player.EyeRotation.Forward * 50)
			.Ignore( player )
			.Run();

		if ( tr.Entity is null )
		{
			isOpen = false;
			CloseShop();
		}

		if (Input.Pressed(InputButton.Use) && lastOpened > 0.3f && tr.Entity is ShopKeeperBase)
		{
			isOpen = !isOpen;
			lastOpened = 0;

			if ( isOpen )
				OpenShop( player, tr.Entity.GetType() );
			else
				CloseShop();
		}

		SetClass( "openshop", isOpen );
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

