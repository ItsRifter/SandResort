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
		BlacklistPanel.AddChild(new BlacklistPanel());
		// BlacklistPanel.AddChild(new BlacklistPanel());
		// receiptionistTabs.AddTab( userSuiteSettingsTab , "Suite");
		receiptionistTabs.AddTab( userSuiteSettingsTab , "Your Condo");
		

		// CHECK OUT TAB
		Panel checkOutTab = Add.Panel("tabSheet tab-checkout");
		checkOutTab.AddChild(new HeaderPanel("Condo Settings."));

		receiptionistTabs.AddTab( checkOutTab , "Condo Settings");


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
			RootPanel.AddChild(new HeaderPanel("Your Condo."));
			Panel SuiteInfo = RootPanel.Add.Panel("SuiteInfo");
			Panel SuiteInfoImage = SuiteInfo.Add.Panel("SuiteInfoImage");

			Panel SuiteInfoStatus = SuiteInfo.Add.Panel("SuiteInfoStatus");
			suiteName =  SuiteInfoStatus.Add.Label("No Name", "suiteText SuiteTitle");
			CheckoutBtn = SuiteInfoStatus.Add.Button("Check out", "checkoutBtn");
		}
	}
	public class BlacklistPanel : Panel
	{
		public Panel BlacklistList;
		public BlacklistPanel()
		{
			Panel RootPanel = Add.Panel("BlacklistPanel");
			RootPanel.AddChild(new HeaderPanel("Blacklist", false));
			BlacklistList = RootPanel.Add.Panel("BlacklistList players");
			BlacklistList.AddChild(new playersList());

			// for (int i = 0; i < 25; i++)
			// {
			// 	Panel fakePlayer = Add.Panel("player");
			// 	fakePlayer.Add.Label("Player " + (i+1), "playerName");
				
			// 	BlacklistList.AddChild(fakePlayer);
			// }
		}
	}
	public partial class plrInfo : Panel {
		private Panel PlayerInfoPanel;
		public Client Client {get; set;}
		public Panel plrPFP;
		public Label plrName;
		// public Label plrID;
		public Panel MoreInfo;
		public bool dropdownOpen = false;
		private Panel MoreInfoPanel;
		public plrInfo(Client cl, Panel MoreInfo = null)
		{
			Client = cl;
			MoreInfoPanel = MoreInfo;
			if (MoreInfoPanel == null)
			{
				MoreInfoPanel = Add.Panel("moreInfo");
				Label emptyText = MoreInfoPanel.Add.Label("Empty", "text");
				emptyText.Style.Set("color", "rgba(255,255,255,0.5)");
				emptyText.Style.Set("font-size", "24px");
			} else {
				MoreInfoPanel = Add.Panel("moreInfo");
				MoreInfoPanel.AddChild(MoreInfo);
				// baseMoreIfno.AddChild(MoreInfoPanel);
			}
			MoreInfoPanel.Style.Set("display", "none");
			PlayerInfoPanel = Add.Panel("playerInfo");
			Panel PlayerInfoSet = PlayerInfoPanel.Add.Panel("playerInfoSet");
			plrPFP = PlayerInfoSet.Add.Panel("PlayerPFP");
			plrPFP.Add.Panel("bkgFade"); 
			Panel PlayerInfo = PlayerInfoSet.Add.Panel("PlayerInfo"); 
			plrName = PlayerInfo.Add.Label(cl.Name, "text name");
			plrPFP.Style.SetBackgroundImage( $"avatarbig:{cl.PlayerId}");
			// plrID = PlayerInfo.Add.Label(cl.PlayerId.ToString(), "text id");
			PlayerInfoSet.AddEventListener("onClick", () => {
				if (dropdownOpen)
				{ dropdownOpen = false; MoreInfoPanel.Style.Set("display", "none"); }
				else
				{ dropdownOpen = true; MoreInfoPanel.Style.Set("display", "flex"); }
			});
			PlayerInfoPanel.AddChild(MoreInfoPanel);
		}
		// plrID = PlayerInfo.Add.Label(cl.PlayerId.ToString(), "text id");
		// plrID.Text = cl.PlayerId.ToString();
	}
    
	public partial class playersList : Panel {
        public Panel players;
        // public Label plrID;
        public playersList()
        {
            players = Add.Panel("players");
            // foreach (var plr in Client.All)
            // {
            //     players.AddChild(new plrInfo(plr));
            // }
        }

        public override void Tick()
        {
            var plyerCount = 0;
            base.Tick();

            foreach (var plrPanel in players.Children.OfType<plrInfo>())
            {
                if (plrPanel.Client.IsValid())
                    continue;
                
                plrPanel.Delete();
                plyerCount--;
            }

            foreach (var plr in Client.All)
            {
                if (players.Children.OfType<plrInfo>().Any( panel => panel.Client == plr ))
                    continue;

                var panel = new plrInfo(plr);
                panel.Parent = players;
                plyerCount++;
            }
        }
    }
}

