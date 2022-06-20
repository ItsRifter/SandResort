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
	private Panel rootSuitesPanel;
	public Panel Tab1;
	public Panel Tab2;

	public Label ReceptionistTitle;
	public Panel Suites;

	SideTabUI TabsUi;


	public SuiteReceptionUI()
    {
		Tab1 = Add.Panel( "tabPanel" );
		Tab2 = Add.Panel( "tabPanel" );
		Tab1.Style.Set( "display: flex;" );
		Tab2.Style.Set( "display: none;" );

		TabsUi = new SideTabUI();

		TabsUi.AddTabItem("Check in", () =>
		{
			Suites.Style.Set( "display: flex;" );
			Tab2.Style.Set( "display: none;" );
		});
		
		TabsUi.AddTabItem("Suite Layout", () =>
		{
			Suites.Style.Set( "display: none;" );
			Tab2.Style.Set( "display: flex;" );
		});


		StyleSheet.Load( "UI/Styles/Lobby/SuiteReceptionUI.scss" );
		rootSuitesPanel = Add.Panel( "mainSuitesPanel" );
		rootSuitesPanel.AddChild( TabsUi );

		Panel titlePanel = rootSuitesPanel.Add.Panel( "titlePanel" );
		ReceptionistTitle = titlePanel.Add.Label( "Suite Receptionist", "title" );
		Suites = rootSuitesPanel.Add.Panel( "suites" );

		Panel TestTabPanel = Tab2.Add.Panel("testPanel");
		TestTabPanel.Add.Label( "lokgpkgkfspgjirwmviorew" );
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
			Panel suite = Suites.Add.Panel( "suite" );
			Panel suiteinner = suite.Add.Panel( "suiteinner" );

			//Image
			suiteinner.Add.Panel( "suiteimage" );

			Panel suiteStatus = suiteinner.Add.Panel( "status" );
			suiteStatus.Add.Label( $"Suite {i + 1}", "title" );
			suiteStatus.Add.Label( "Vacant or not", "ownership" );

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

