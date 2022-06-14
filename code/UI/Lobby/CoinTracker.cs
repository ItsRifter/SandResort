using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

public partial class CoinTracker : Panel
{
	public Panel CoinPnl;
	public Label CoinLbl;

	public CoinTracker()
	{
		StyleSheet.Load( "UI/Styles/Lobby/CoinTracker.scss" );

		CoinPnl = Add.Panel();
		CoinLbl = CoinPnl.Add.Label("???");
	}

	public override void Tick()
	{
		base.Tick();

		var player = Local.Client.Pawn as PHPawn;

		if ( player == null ) 
			return;

		CoinLbl.SetText( $"{player.PlayCoins} PlayCoins" );

	}
}


