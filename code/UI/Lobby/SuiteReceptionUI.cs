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
	public Label ReceptionistTitle;
	public Panel Suites;

	public SuiteReceptionUI()
    {
        StyleSheet.Load( "UI/Styles/Lobby/SuiteReceptionUI.scss" );

		rootSuitesPanel = Add.Panel( "mainSuitesPanel" );
		Panel titlePanel = rootSuitesPanel.Add.Panel( "titlePanel" );
		ReceptionistTitle = titlePanel.Add.Label( "Suite Receptionist", "title" );
		Suites = rootSuitesPanel.Add.Panel( "suites" );
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
			suiteStatus.Add.Label( "suite test", "title" );
			suiteStatus.Add.Label( $"Suite {i + 1}", "ownership" );

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

