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

		// BEING REPLACED //
		//CoinPnl = Add.Panel("mainCoinPanel");
		//CoinLbl = Add.Label("???", "gradiant-coins");
		//Add.Label( "Alex help" );
		//// -- //
		//keyboardActions = CoinPnl.Add.Panel("keyboardActions");
		//// F: Pay respect //
		//keyboardActions.AddChild(CreateKeyUI("F", "now i am in a void panel"));

		// -- //
	}

	//public Panel CreateKeyUI(string keyTest, string actionText)
    //{
	//	Panel action = Add.Panel("keyAction");
	//	Panel action1key = action.Add.Panel("key"); action1key.Add.Label(keyTest);
	//	action.Add.Label(actionText);
	//
	//	return action;
	//}

	public override void Tick()
	{
		base.Tick();

		var player = Local.Pawn as PHPawn;

		if ( player == null ) 
			return;

		CoinLbl.Text = string.Format($"{player.PlayCoins:C0}");
		//CoinLbl.SetText( $"{player.PlayCoins}" );

		Username.SetText( Local.Client.Name );

		Avatar.Style.SetBackgroundImage( $"avatar:{Local.Client.PlayerId}" );
	}
}


