using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

public partial class Inventory : Panel
{
	public Panel InvPanel;

	public Panel InventoryBar;
	public Panel InvBag;
	public Panel MainBag;

	bool isOpen;

	List<string> holdingItems = new List<string>();

	public Inventory()
	{
		StyleSheet.Load( "UI/Styles/Lobby/Inventory.scss" );
		
		isOpen = false;

		InventoryBar = Add.Panel("invBar");

		//itemTest = InventoryBar.Add.Panel("quickItem");

		InventoryBar.Add.Panel("quickItem");
		InventoryBar.Add.Panel("quickItem");
		InventoryBar.Add.Panel("quickItem");
		InventoryBar.Add.Panel("quickItem");
		InventoryBar.Add.Panel("quickItem");

		InvBag = Add.Panel( "invBag" );
		InvBag.Add.Label("Inventory", "invText");

		MainBag = InvBag.Add.Panel("mainBag");

		for( int i = 0; i < 20; i++ )
		{
			MainBag.Add.Panel("bagItem");
		}

	}

	public void SetInventorySlots(PHPawn player)
	{
		int index = 0;

		foreach ( var item in player.PHInventory.ClientInventory )
		{
			var displayItem = TypeLibrary.Create(TypeLibrary.GetTypeByName(item).FullName, TypeLibrary.GetTypeByName(item)) as PHSuiteProps;

			holdingItems.Add( displayItem.GetType().FullName );

			MainBag.GetChild( index ).Style.SetBackgroundImage( displayItem.SuiteItemImage );

			if( holdingItems[index] != null )
			{
				MainBag.GetChild( index ).AddEventListener( "onclick", () =>
				{
					DragItem( holdingItems[index] );
					MainBag.GetChild( index ).Style.SetBackgroundImage( "" );
					holdingItems.RemoveAt( index );

				} );
			}

			displayItem.Delete();
			index++;
		}
	}

	public void DragItem(string prop)
	{
		ConsoleSystem.Run( "ph_drag_item", prop );
	}

	public override void Tick()
	{
		base.Tick();

		var player = Local.Pawn as PHPawn;

		if (player == null)
			return;

		//itemTest.Style.SetBackgroundImage($"avatar:{Local.Client.PlayerId}");

		if (Input.Pressed(InputButton.Menu))
		{
			isOpen = true;
			SetInventorySlots( player );
		}
		else if (Input.Released(InputButton.Menu))
		{
			isOpen = false;
		}

		InvBag.SetClass( "openBag", isOpen );
		InventoryBar.SetClass( "openQuick", isOpen );
	}
}

