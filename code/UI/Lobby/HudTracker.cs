using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

public partial class HudTracker : Panel
{
	public Panel hud;
	public Panel keyboardActions;
	public Label CoinLbl;
	public Label Username;
	public Panel Avatar;
	public HudTracker()
	{
		StyleSheet.Load( "UI/Styles/Lobby/HudTracker.scss" );

		hud = Add.Panel( "hud" ); 
		Panel mainContainer = hud.Add.Panel( "main-cointracker" );
		mainContainer.Add.Panel( "logo" );
		Panel cointracker = mainContainer.Add.Panel("coinTracker");

		Panel coins = cointracker.Add.Panel( "coins" );
		coins.Add.Panel( "html_coin" );
		CoinLbl = coins.Add.Label("???", "amount" );
		Panel panelUsername = cointracker.Add.Panel("username");
		Username = panelUsername.Add.Label( "username goes here", "name" );
		mainContainer.Add.Panel("gradient_topright");
		mainContainer.Add.Panel("gradient_bottomleft");

		Avatar = hud.Add.Panel( "logo" );
	}

	public override void Tick()
	{
		base.Tick();

		var player = Local.Pawn as LobbyPawn;
	
		SetClass( "active", player != null );

		if ( player == null ) 
			return;

		CoinLbl.Text = string.Format($"{player.PlayCoins:C0}");

		Username.SetText( Local.Client.Name );

		Avatar.Style.SetBackgroundImage( $"avatarbig:{Local.Client.PlayerId}" );
	}
}


