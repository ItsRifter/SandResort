using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using ComplexUI;

public partial class SuiteReceptionUI : Panel
{
	bool isOpen = false;
	Panel rootSuitesPanel;
	public Panel Suites;

	SideTabUI TabsUi;
	Panel checkInSuite;
	Panel userSuite;

	public Label suiteName;
	public Label suiteValue;
	public Button Checkoutbtn;
	public Panel suiteImage;

	public Panel userSuitePlayersInSuite;
	public Panel userSuiteBlacklist;

	public SuiteReceptionUI()
    {
		// ! ALL UI WILL BE UPDATED IN THE FUTURE ! // INTERFACE ICELANDIC //
		StyleSheet.Load( "UI/Styles/Lobby/SuiteReceptionUI.scss" );

		TabsUi = new SideTabUI();

		TabsUi.AddTabItem("Check in", () =>
		{
			checkInSuite.Style.Set( "display: flex;" );
			userSuite.Style.Set( "display: none;" );
		});
		
		TabsUi.AddTabItem("Suite Settings", () =>
		{
			checkInSuite.Style.Set( "display: none;" );
			userSuite.Style.Set( "display: flex;" );
		});

		TabsUi.AddTabItem( "Check Out", () =>
		{
			ConsoleSystem.Run( "sc_checkout_suite", Local.Client.Id );
			CloseSuiteMenu();
		}, false );

		//UI

		rootSuitesPanel = Add.Panel( "mainSuitesPanel" );
		rootSuitesPanel.AddChild( TabsUi );

		//root
		checkInSuite = rootSuitesPanel.Add.Panel("checkinSuites");
		userSuite = rootSuitesPanel.Add.Panel("tabuserSuite");
		
		//check in
		Panel titlePanel = checkInSuite.Add.Panel( "titlePanel" );
		titlePanel.Add.Label( "Suite Receptionist", "title" );
		checkInSuite.Add.Panel("seperator");
		Suites = checkInSuite.Add.Panel( "suites" );
		
		//user suite
		Panel titlePanel_usersuite = userSuite.Add.Panel("titlePanel");
		titlePanel_usersuite.Add.Label("Your suite", "title");
		userSuite.Add.Panel("seperator");
		
		Panel SuiteContainer     =	userSuite.Add.Panel("suite-container");
		Panel MainSuite			 =	SuiteContainer.Add.Panel("suite");
		Panel MainSuite_inner	 =	MainSuite.Add.Panel( "suite-inner" );   // inner container
		suiteImage				 =	MainSuite_inner.Add.Panel("suite-img"); // image of the suite
		Panel suiteInfoContainer =	MainSuite_inner.Add.Panel("suite-info");// info about the suite

		// Your suite / Selected suite preview

		suiteName = suiteInfoContainer.Add.Label( "No suite", "suite-title" );
		suiteValue = suiteInfoContainer.Add.Label( "Value: $69420" , "value");
		Checkoutbtn = suiteInfoContainer.Add.Button("Check out", "checkout");

		MainSuite.Add.Panel( "suite-bkg-image" );

		userSuite.Add.Panel("seperator");

		// Your suite / Players suite info

		Panel playersSuite = userSuite.Add.Panel("suitePlayerList");

		// Your suite / Players suite info / Players in suite

		Panel PlayersInSuiteContainer = playersSuite.Add.Panel("list ListPlayersInSuite");
		PlayersInSuiteContainer.Add.Label( "Players in your suite", "header" );
		userSuitePlayersInSuite = PlayersInSuiteContainer.Add.Panel("players");

		// Your suite / Players suite info / Players blacklisted from suite

		Panel blacklistedContainer = playersSuite.Add.Panel("list listBlacklist");
		blacklistedContainer.Add.Label( "Blacklist", "header");
		//blacklistedContainer.Add.Label( "Block people from entering your suite", "info");
		userSuiteBlacklist = blacklistedContainer.Add.Panel( "players" );

		// Tab final styling
		checkInSuite.Style.Set("display: flex;");
		userSuite.Style.Set("display: none;");

		//TabsUi.SetTabItem(0, true);
	}

	public void ClaimSuitePanel( int index )
	{
		ConsoleSystem.Run( "sc_claim_suite", index, Local.Client.Id );
		CloseSuiteMenu();
	}

	public void OpenSuiteMenu(LobbyPawn player)
	{
		if ( isOpen )
			return;

		//TabsUi.SetTabItem( 1, true );
		//TabsUi.SetTabItem( 2, false );

		Suites.DeleteChildren();
		userSuitePlayersInSuite.DeleteChildren();
		userSuiteBlacklist.DeleteChildren();

		Style.ZIndex = 5;

		int[] suiteIndex = new int[4]
		{
			0, 1, 2, 3
		};

		//For indexes don't work for some reason... this does?
		//for loops returns a 4 weirdly

		foreach ( int i in suiteIndex )
		{
			var newSuite = player.GrabAllSuites()[i];

			Panel suite = Suites.Add.Panel( "suite" );
			Panel suiteinner = suite.Add.Panel( "suiteinner" );

			//Image
			var suiteBG = suiteinner.Add.Panel( "suiteimage" );

			Panel suiteStatus = suiteinner.Add.Panel( "status" );
			suiteStatus.Add.Label( $"Suite {i+1}", "title" );

			if ( newSuite.SuiteOwner != null )
			{
				suiteStatus.Add.Label( $"{newSuite.SuiteOwner.PlayerName}'s Suite", "ownership" );
				suiteBG.Style.BackgroundImage = Texture.Load( FileSystem.Mounted, "ui/suitebasic.jpg" );
			}
			else
			{
				suiteStatus.Add.Label( "Claim Suite", "ownership" );
				suiteBG.Style.BackgroundImage = Texture.Load( FileSystem.Mounted, "ui/suitebasic.jpg");
				suiteBG.Add.Label( "Vacant", "textVacant" );
			}

			suite.AddEventListener( "onclick", () =>
			{
				ClaimSuitePanel( i );
			} );
		}

		if( player.GetPawnsInSuite() != null)
		{
			//Players currently in suite
			foreach ( var pawn in player.GetPawnsInSuite() )
			{
				Panel Player = Add.Panel( "Player" );
				Panel info = Player.Add.Panel( "info" );
				Panel actions = Player.Add.Panel( "actions" );

				Panel icon = info.Add.Panel( "pfp" );
				info.Add.Label( $"{pawn.Client.Name}", "name" );

				icon.Style.SetBackgroundImage( $"avatar:{pawn.Client.PlayerId}" );

				Panel blacklist = actions.Add.Button( "Blacklist", "action" );
				Panel kick = actions.Add.Button( "Kick", "action" );

				userSuitePlayersInSuite.AddChild( Player );

				kick.AddEventListener("onclick", () => 
				{
					ConsoleSystem.Run("sc_suite_kick", pawn.Client.Name);
				});

				blacklist.AddEventListener( "onclick", () =>
				{
					ConsoleSystem.Run( "sc_suite_blacklist_add", pawn.Client.Name );
					ConsoleSystem.Run( "sc_suite_kick", pawn.Client.Name );

				} );
			}

			//Blacklist
			foreach ( var pawn in player.GetSuiteBlacklist() )
			{
				Panel Player = Add.Panel( "Player blacklisted" );
				Panel info = Player.Add.Panel( "info" );
				Panel actions = Player.Add.Panel( "actions" );

				var removeBtn = actions.Add.Button( "Remove", "action blacklistAction" );

				Panel icon = info.Add.Panel( "pfp" );
				info.Add.Label( $"{pawn.Client.Name}", "name" );

				icon.Style.SetBackgroundImage( $"avatar:{pawn.Client.PlayerId}" );

				removeBtn.AddEventListener( "onclick", () =>
				{
					ConsoleSystem.Run( "sc_suite_blacklist_remove", pawn.Client.Name );
				} );

				userSuiteBlacklist.AddChild( Player );
			}
		}

		isOpen = true;
	}

	public void CloseSuiteMenu()
	{
		isOpen = false;
		Style.ZIndex = 0;

	}

	public override void Tick()
	{
		base.Tick();

		if ( Local.Pawn is not LobbyPawn player )
			return;

		if( player.InteractNPC == null )
			CloseSuiteMenu();

		if (player.InteractNPC is SuiteReceptionist && !isOpen && Input.Pressed(InputButton.Use) )
			OpenSuiteMenu( player ); 
		else if ( isOpen && Input.Pressed( InputButton.Use ) )
			CloseSuiteMenu();

		rootSuitesPanel.SetClass("open", isOpen);
	
	}
}

