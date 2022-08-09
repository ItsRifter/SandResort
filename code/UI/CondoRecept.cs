using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

public class CondoRecept : Panel
{
	bool isOpen = false;

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

		if(Input.Pressed(InputButton.Menu))
		{
			switch( isOpen )
			{
				case true:
					CloseReceiptionMenu();
					break;
				case false:
					OpenReceiptonMenu();
					break;
			}
		}
	}
}

