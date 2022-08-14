using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using SandCasle.UI;

public class CondoRecept : Panel
{
	public Panel ReceptRootPanel;
    public TimeSince lastOpen = 0;
	public Panel BlacklistPanel;

	public CondoRecept()
	{
		SetClass("open", true);
		StyleSheet.Load( "UI/Styles/Lobby/CondoRecept.scss" );
		StyleSheet.Load( "UI/Styles/Lobby/CSUIContruct.scss" );
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

		ReceptRootPanel.AddChild(receiptionistTabs);

		AddChild( new CloseButton(() =>
		{
			CloseReceptionist();
		} ));
	}

	public override void Tick()
	{
		base.Tick();
	}

	public void CloseReceptionist()
	{
		this.Delete();
	}
}

namespace Sandbox.UI
{
	public class BlacklistPanel : Panel
	{
		public Panel BlacklistList;
		public BlacklistPanel()
		{
			Panel RootPanel = Add.Panel( "BlacklistPanel" );
			RootPanel.AddChild( new HeaderPanel( "Blacklist", false ) );
			BlacklistList = RootPanel.Add.Panel( "BlacklistList players" );
			BlacklistList.AddChild( new playersList() );
		}
	}
	public class plrInfo : Panel
	{
		private Panel PlayerInfoPanel;
		public Client Client { get; set; }
		public Panel plrPFP;
		public Label plrName;
		// public Label plrID;
		public Panel MoreInfo;
		public bool dropdownOpen = false;
		private Panel MoreInfoPanel;
		public plrInfo( Client cl, Panel MoreInfo = null )
		{
			Client = cl;
			MoreInfoPanel = MoreInfo;
			if ( MoreInfoPanel == null )
			{
				MoreInfoPanel = Add.Panel( "moreInfo" );
				Label emptyText = MoreInfoPanel.Add.Label( "Empty", "text" );
				emptyText.Style.Set( "color", "rgba(255,255,255,0.5)" );
				emptyText.Style.Set( "font-size", "24px" );
			}
			else
			{
				MoreInfoPanel = Add.Panel( "moreInfo" );
				MoreInfoPanel.AddChild( MoreInfo );
				// baseMoreIfno.AddChild(MoreInfoPanel);
			}
			MoreInfoPanel.Style.Set( "display", "none" );
			PlayerInfoPanel = Add.Panel( "playerInfo" );
			Panel PlayerInfoSet = PlayerInfoPanel.Add.Panel( "playerInfoSet" );
			plrPFP = PlayerInfoSet.Add.Panel( "PlayerPFP" );
			plrPFP.Add.Panel( "bkgFade" );
			Panel PlayerInfo = PlayerInfoSet.Add.Panel( "PlayerInfo" );
			plrName = PlayerInfo.Add.Label( cl.Name, "text name" );
			plrPFP.Style.SetBackgroundImage( $"avatarbig:{cl.PlayerId}" );
			// plrID = PlayerInfo.Add.Label(cl.PlayerId.ToString(), "text id");
			PlayerInfoSet.AddEventListener( "onClick", () => {
				if ( dropdownOpen )
				{ dropdownOpen = false; MoreInfoPanel.Style.Set( "display", "none" ); }
				else
				{ dropdownOpen = true; MoreInfoPanel.Style.Set( "display", "flex" ); }
			} );
			PlayerInfoPanel.AddChild( MoreInfoPanel );
		}
		// plrID = PlayerInfo.Add.Label(cl.PlayerId.ToString(), "text id");
		// plrID.Text = cl.PlayerId.ToString();
	}

	public partial class playersList : Panel
	{
		public Panel players;
		// public Label plrID;
		public playersList()
		{
			players = Add.Panel( "players" );
			// foreach (var plr in Client.All)
			// {
			//     players.AddChild(new plrInfo(plr));
			// }
		}

		public override void Tick()
		{
			var plyerCount = 0;
			base.Tick();

			foreach ( var plrPanel in players.Children.OfType<plrInfo>() )
			{
				if ( plrPanel.Client.IsValid() )
					continue;

				plrPanel.Delete();
				plyerCount--;
			}

			foreach ( var plr in Client.All )
			{
				if ( players.Children.OfType<plrInfo>().Any( panel => panel.Client == plr ) )
					continue;

				var panel = new plrInfo( plr );
				panel.Parent = players;
				plyerCount++;
			}
		}
	}
}


