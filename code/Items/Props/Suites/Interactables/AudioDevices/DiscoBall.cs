using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;

public partial class DiscoBall : PHSuiteProps
{
	public override string SuiteItemName => "Disco Ball";
	public override string SuiteItemDesc => "A disco ball that can play music";
	public override string SuiteItemImage => "ui/sc_icon_discoball.png";
	public override Model WorldModel => Model.Load( "models/clutter/discoball.vmdl" );
	public override int SuiteItemCost => 5000;
	public override ShopType ShopSeller => ShopType.Electric;

	Sound playingSound;

	public override void Interact( LobbyPawn player )
	{
		base.Interact( player );

		if ( IsClient )
			return;

		if ( playingSound.Finished )
		{
			int random = Rand.Int( 1, 2 );
			if( random == 1)
				playingSound = PlaySound( "interact_classicradio" );
			else if ( random == 2 )
				playingSound = PlaySound( "interact_classicradio_2" );
		}
		else if ( !playingSound.Finished )
		{
			playingSound.Stop();
		}
		
	}
}

