using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;
public partial class BeerBottle : PHSuiteProps
{
	public override string SuiteItemName => "Bottle of Beer";
	public override string SuiteItemDesc => "A bottle containing alcohol, drink responsibly";
	public override string SuiteItemImage => "ui/ph_icon_beerbottle.png";
	public override Model WorldModel => Model.Load( "models/clutter/glassbottle/glassbottle.vmdl" );
	public override int SuiteItemCost => 25;
	public override ShopType ShopSeller => ShopType.Bar;

	public override void Interact( PHPawn player )
	{
		base.Interact( player );

		player.Drunkiness += 15.0f;
		player.TimeLastDrank = 0;

		if ( player.Drunkiness >= 100.0f )
		{
			player.OnKilled();
			player.TimeLastDrank = 0;
		}

		Delete();
	}
}
