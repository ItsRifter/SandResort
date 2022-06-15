using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;

public partial class ClassicRadio : PHSuiteProps
{
	public override string SuiteItemName => "Classical Radio";
	public override string SuiteItemDesc => "An old time radio, only has one channel still available";
	public override Model WorldModel => Model.Load( "models/radio/oldradio/oldradio.vmdl" );
	public override int SuiteItemCost => 250;
	public override ShopType ShopSeller => ShopType.Electric;

	Sound playingSound;

	public override void Interact( PHPawn player )
	{
		base.Interact( player );

		if( playingSound.Finished )
			playingSound = PlaySound( "interact_classicradio" );
	}
}

