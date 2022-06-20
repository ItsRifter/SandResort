using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using Sandbox.UI.Tests;

[Library]
public partial class Inventory : Panel
{
	public Panel InvPanel;

	public Panel InventoryBar;
	public Panel InvBag;
	public Panel MainBag;

	PHSuiteProps lastProp;

	bool isOpen;
	TimeSince waitToReopenMenu;

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

		waitToReopenMenu = 0;

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
		if ( player.PHInventory.ClientInventory.Count < 0 )
		{
			ConsoleSystem.Run( "ph_qmenu_reset" );
			ResetInventorySlots();
		}
	
		int index = 0;
		
		foreach ( var item in player.PHInventory.ClientInventory )
		{
			var createItem = TypeLibrary.Create<PHSuiteProps>( item );

			MainBag.GetChild( index ).Style.SetBackgroundImage( createItem.SuiteItemImage );

			MainBag.GetChild( index ).AddEventListener( "onclick", () =>
			{
				DragItem( createItem.ClassName );
			} );

			createItem.Delete();

			index++;
		}
	}

	[Event.BuildInput]
	public void BuildInput(InputBuilder inputBuild)
	{
		if ( Local.Pawn is not PHPawn player )
			return;

		if ( !isOpen )
			return;

		if( lastProp == null && player.PreviewProp != null )
		{
			lastProp = player.PreviewProp;
		}
	
		var mouseTR = Trace.Ray( inputBuild.Cursor.Origin, inputBuild.Cursor.Project( 180 ) )
			.Ignore(player)
			.Run();

		InvBag.SetClass( "allowPointerEvents", mouseTR.Entity is not PHSuiteProps && lastProp == null && isOpen );
		InventoryBar.SetClass( "allowPointerEvents", mouseTR.Entity is not PHSuiteProps && lastProp == null && isOpen );
		
		SetClass( "allowPointerEvents", (mouseTR.Entity is PHSuiteProps || lastProp != null) && isOpen );
		
		if ( mouseTR.Entity is PHSuiteProps hovering && player.PreviewProp == null && Input.Pressed( InputButton.PrimaryAttack ) && hovering.PropOwner == player )
		{
			ResetInventorySlots();
			ConsoleSystem.Run( "ph_drag_item", hovering.GetType().FullName, hovering.Name );
			lastProp = hovering;
		}	

	}

	public void DragItem(string prop)
	{
		ConsoleSystem.Run( "ph_select_item", prop );
	}

	public override void Tick()
	{
		base.Tick();

		if ( Local.Pawn is not PHPawn player )
			return;

		if (Input.Pressed(InputButton.Menu) )
		{
			if ( waitToReopenMenu <= 0.15f )
				return;

			Style.ZIndex = 5;

			isOpen = true;
			SetInventorySlots( player );
		}
		else if (Input.Released(InputButton.Menu))
		{
			ConsoleSystem.Run( "ph_qmenu_clear" );

			SetClass( "allowPointerEvents", false );
			InventoryBar.SetClass( "allowPointerEvents", false );
			InvBag.SetClass( "allowPointerEvents", false );

			lastProp = null;
			isOpen = false;

			Style.ZIndex = 0;

			waitToReopenMenu = 0;
		}

		if ( Input.Pressed( InputButton.PrimaryAttack ) && player.PreviewProp != null )
		{
			isOpen = false;

			InventoryBar.SetClass( "allowPointerEvents", false );
			InvBag.SetClass( "allowPointerEvents", false );
			ResetInventorySlots();

			Style.ZIndex = 0;

			waitToReopenMenu = 0;
		}

		if (Input.Pressed(InputButton.SecondaryAttack) && player.PreviewProp != null)
		{
			isOpen = false;

			InventoryBar.SetClass( "allowPointerEvents", false );
			InvBag.SetClass( "allowPointerEvents", false );
			SetClass( "allowPointerEvents", false );
			lastProp = null;

			Style.ZIndex = 0;

			waitToReopenMenu = 0;
		}

		InvBag.SetClass( "openBag", isOpen );
		InventoryBar.SetClass( "openQuick", isOpen );

	}
}

