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
	public Panel BlacklistPanel;

	public CondoRecept()
	{
		SetClass("open", isOpen);
		StyleSheet.Load( "UI/Styles/Lobby/CondoRecept.scss" );
		ReceptRootPanel = Add.Panel("ReceptRootPanel");

		TabContainer receiptionistTabs = new TabContainer();

		// CHECK IN TAB
		Panel checkInTab = Add.Panel("tabSheet tab-checkin");
		Panel checkInTabScroll = checkInTab.Add.Panel("tabSheetScroll");
		checkInTabScroll.AddChild(new HeaderPanel("Check in."));

		receiptionistTabs.AddTab( checkInTab , "Check in");

		// USER SUITE SETTINGS TAB
		Panel userSuiteSettingsTab = Add.Panel("tabSheet tab-usersuitesettings");
		Panel userSuiteSettingsTabScroll = userSuiteSettingsTab.Add.Panel("tabSheetScroll");
		userSuiteSettingsTabScroll.AddChild(new ClientSuitePanel());
		BlacklistPanel = userSuiteSettingsTabScroll.Add.Panel("blacklist");
		BlacklistPanel.AddChild(new HeaderPanel("Blacklist", false));
		BlacklistPanel.Add.Label("// TODO Make blacklist list", "blacklist-label");
		// BlacklistPanel.AddChild(new BlacklistPanel());
		// receiptionistTabs.AddTab( userSuiteSettingsTab , "Suite");
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
		SetClass("open", true);
	}

	public void CloseReceiptionMenu()
	{
		if ( !isOpen )
			return;

		Log.Info( "Close receiptionist menu" );

		isOpen = false;
		SetClass("open", false);
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
		public Label textLabel;
		public HeaderPanel(String title, bool haveMarginTop = true)
		{
			Panel MainHeaderPanel = Add.Panel("HeaderPanel");
			textLabel = MainHeaderPanel.Add.Label(title, "headerTitle");
			MainHeaderPanel.Add.Panel("headerSeparator");

			if (haveMarginTop) {
				AddClass("addMarginTop");
			}
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
			Panel RootPanel = Add.Panel("ClientSuitePanel");
			SuiteBackground = RootPanel.Add.Panel("SuiteBackground");
			RootPanel.AddChild(new HeaderPanel("Your Suite."));
			Panel SuiteInfo = RootPanel.Add.Panel("SuiteInfo");
			Panel SuiteInfoImage = SuiteInfo.Add.Panel("SuiteInfoImage");

			Panel SuiteInfoStatus = SuiteInfo.Add.Panel("SuiteInfoStatus");
			suiteName =  SuiteInfoStatus.Add.Label("No Name", "suiteText SuiteTitle");
			CheckoutBtn = SuiteInfoStatus.Add.Button("Check out", "checkoutBtn");
		}
	}
}

