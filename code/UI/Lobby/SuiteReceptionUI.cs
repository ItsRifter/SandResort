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
	public Button Checkout;

	public SuiteReceptionUI()
    {
		StyleSheet.Load( "UI/Styles/Lobby/SuiteReceptionUI.scss" );

		TabsUi = new SideTabUI();

		TabsUi.AddTabItem("Check in", () =>
		{
			checkInSuite.Style.Set( "display: flex;" );
			userSuite.Style.Set( "display: none;" );
		});
		
		TabsUi.AddTabItem("Suite Layout", () =>
		{
			checkInSuite.Style.Set( "display: none;" );
			userSuite.Style.Set( "display: flex;" );
		});

		// UI

		rootSuitesPanel = Add.Panel( "mainSuitesPanel" );
		rootSuitesPanel.AddChild( TabsUi );

		// root
		checkInSuite = rootSuitesPanel.Add.Panel("checkinSuites");
		userSuite = rootSuitesPanel.Add.Panel("tabuserSuite");
		//
		// // check in // //
		Panel titlePanel = checkInSuite.Add.Panel( "titlePanel" );
		titlePanel.Add.Label( "Suite Receptionist", "title" );
		checkInSuite.Add.Panel("seperator");
		Suites = checkInSuite.Add.Panel( "suites" );
		//
		// // user suite // //
		Panel titlePanel_usersuite = userSuite.Add.Panel("titlePanel");
		titlePanel_usersuite.Add.Label("Your suite", "title");
		userSuite.Add.Panel("seperator");
		//
		Panel SuiteContainer =		userSuite.Add.Panel("suite-container");
		Panel MainSuite =			SuiteContainer.Add.Panel("suite");
		Panel MainSuite_inner =		MainSuite.Add.Panel( "suite-inner" );   // inner container
		Panel suiteImage =			MainSuite_inner.Add.Panel("suite-img"); // image of the suite
		Panel suiteInfoContainer =	MainSuite_inner.Add.Panel("suite-info");// info about the suite
																			//

		suiteInfoContainer.Add.Label( "No suite", "suite-title" );
		suiteInfoContainer.Add.Label( "Value: $69420" , "value");
		suiteInfoContainer.Add.Button("No suite to check out", "checkout");

		userSuite.Add.Panel("seperator");

		// Tab final styling
		checkInSuite.Style.Set("display: flex;");
		userSuite.Style.Set("display: none;");
	}

	public void ClaimSuitePanel( int index )
	{
		ConsoleSystem.Run( "ph_claim_suite", index, Local.Client.Id );
		CloseSuiteMenu();
	}

	public void OpenSuiteMenu()
	{
		if ( isOpen )
			return;

		//TabsUi.SetTabItem( 1, true );
		//TabsUi.SetTabItem( 2, false );

		Suites.DeleteChildren();

		Style.ZIndex = 5;

		int[] suiteIndex = new int[4]
		{
			0, 1, 2, 3
		};

		//For indexes don't work for some reason... this does?
		//for loops returns a 4 weirdly

		foreach ( int i in suiteIndex )
		{
			var newSuite = PHGame.Instance.GrabAllSuites()[i];

			Panel suite = Suites.Add.Panel( "suite" );
			Panel suiteinner = suite.Add.Panel( "suiteinner" );

			//Image
			suiteinner.Add.Panel( "suiteimage" );

			Panel suiteStatus = suiteinner.Add.Panel( "status" );
			suiteStatus.Add.Label( $"Suite {i+1}", "title" );

			if( newSuite.SuiteOwner != null )
				suiteStatus.Add.Label( $"{newSuite.SuiteOwner.PlayerName}'s Suite", "ownership" );
			else
				suiteStatus.Add.Label( "Vacant", "ownership" );

			suite.AddEventListener( "onclick", () =>
			{
				ClaimSuitePanel( i );
			} );
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

		if ( Local.Pawn is not PHPawn player )
			return;

		if( player.InteractNPC == null )
			CloseSuiteMenu();

		if (player.InteractNPC is SuiteReceptionist && !isOpen && Input.Pressed(InputButton.Use) )
			OpenSuiteMenu(); 
		else if ( isOpen && Input.Pressed( InputButton.Use ) )
			CloseSuiteMenu();

		rootSuitesPanel.SetClass("open", isOpen);
	
	}
}

