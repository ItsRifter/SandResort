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
	float scrollRot = 0.0f;

	public Inventory()
	{
		StyleSheet.Load( "UI/Styles/Lobby/Inventory.scss" );
		
		isOpen = false;

		InventoryBar = Add.Panel("invBar");

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

	public void ResetInventorySlots()
	{
		MainBag.DeleteChildren();

		for ( int i = 0; i < 20; i++ )
		{
			MainBag.Add.Panel( "bagItem" );
		}
	}

	public void SetInventorySlots(PHPawn player)
	{
		if ( player.PHInventory.ClientInventory.Count <= 0 )
		{
			ResetInventorySlots();
			return;
		}
		
		int index = 0;
		
		foreach ( var item in player.PHInventory.ClientInventory )
		{
			MainBag.GetChild( index ).Style.SetBackgroundImage( item.Item2 );

			MainBag.GetChild( index ).AddEventListener( "onclick", () =>
			{
				DragItem( item.Item1 );
				
			/*	foreach ( var bagItem in MainBag.Children)
				{
					if ( bagItem.Style.BackgroundImage == null)
						continue;

					bagItem.Style.SetBackgroundImage( "" );
				}*/

			} );

			index++;
		}
	}

	[Event.BuildInput]
	public void BuildInput(InputBuilder inputBuild)
	{
		if ( Local.Pawn is not PHPawn player )
			return;

		if( isOpen && player.PreviewProp == null )
		{
			var mouseTR = Trace.Ray( inputBuild.Cursor.Origin, inputBuild.Cursor.Project( 180 ) )
				.Ignore(player)
				.Run();

			if(mouseTR.Entity != null && mouseTR.Entity is PHSuiteProps prop && !prop.IsPreview)
			{
				ConsoleSystem.Run( "ph_drag_item", prop.GetType().FullName, prop.Name );
				isOpen = false;
			}
		}
	}

	public void DragItem(string prop)
	{
		ConsoleSystem.Run( "ph_select_item", prop );
		ResetInventorySlots();
		isOpen = false;
	}

	public override void Tick()
	{
		base.Tick();

		if ( Local.Pawn is not PHPawn player )
			return;

		if (Input.Pressed(InputButton.Menu))
		{
			isOpen = true;
			SetInventorySlots( player );
		}
		else if (Input.Released(InputButton.Menu))
		{
			ConsoleSystem.Run( "ph_qmenu_clear" );
			isOpen = false;
		}

		InvBag.SetClass( "openBag", isOpen );
		InventoryBar.SetClass( "openQuick", isOpen );
	}
}

