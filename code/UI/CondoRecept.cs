using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using SC.UI.Construct;

public class CondoRecept : Panel
{
	public bool isOpen = false;
	public Panel ReceptRootPanel;
    public TimeSince lastOpen = 0;

	public CondoRecept()
	{
		SetClass("open", isOpen);
		StyleSheet.Load( "UI/Styles/Lobby/CondoRecept.scss" );
		ReceptRootPanel = Add.Panel("ReceptRootPanel");

		TabContainer receiptionistTabs = new TabContainer();

		// CHECK IN TAB
		Panel checkInTab = Add.Panel("tabSheet tab-checkin");
		checkInTab.AddChild(new HeaderPanel("Check in."));

		receiptionistTabs.AddTab( checkInTab , "Check in");

		// USER SUITE SETTINGS TAB
		Panel userSuiteSettingsTab = Add.Panel("tabSheet tab-usersuitesettings");
		userSuiteSettingsTab.AddChild(new HeaderPanel("Your Suite."));

		receiptionistTabs.AddTab( userSuiteSettingsTab , "Your Suite");

		// CHECK OUT TAB
		Panel checkOutTab = Add.Panel("tabSheet tab-checkout");
		checkOutTab.AddChild(new HeaderPanel("Check out."));

		receiptionistTabs.AddTab( checkOutTab , "Check out");


		// ADD CHILD
		ReceptRootPanel.AddChild(receiptionistTabs);
	}

	public void OpenReceiptonMenu()
	{
		if ( isOpen )
			return;

		Log.Info( "Open receiptionist menu" );

		isOpen = true;
	}

	public void CloseReceiptionMenu()
	{
		if ( !isOpen )
			return;

		Log.Info( "Close receiptionist menu" );

		isOpen = false;
	}

	public override void Tick()
	{
		base.Tick();

		if(lastOpen > 0.1 && Input.Pressed(InputButton.Menu))
		{
			lastOpen = 0;
			isOpen = !isOpen;
			SetClass("open", isOpen);
			Log.Info( $"receiptionist menu open: {isOpen}" );
		}
	}
}

namespace SC.UI.Construct
{
	public class HeaderPanel : Panel
	{
		public HeaderPanel(String title)
		{
			Panel MainHeaderPanel = Add.Panel("HeaderPanel");
			MainHeaderPanel.Add.Label(title, "headerTitle");
			MainHeaderPanel.Add.Panel("headerSeparator");
		}
	}
}

