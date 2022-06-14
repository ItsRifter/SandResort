using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

public partial class Shop : Panel
{
	bool isOpen;
	TimeSince lastOpened;

	public Shop()
	{
		StyleSheet.Load( "UI/Styles/Lobby/Shop.scss" );
	}

	public override void Tick()
	{
		base.Tick();

		if(Input.Pressed(InputButton.Menu) && lastOpened > 0.3f)
		{
			isOpen = !isOpen;
			lastOpened = 0;
		}

		SetClass( "openshop", isOpen );
	}
}

