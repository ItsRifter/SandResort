using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;
public partial class BeerBarrel : PHSuiteProps
{
	public override string SuiteItemName => "Barrel of Beer";
	public override string SuiteItemDesc => "A barrel of beer, don't get too drunk";
	public override string SuiteItemImage => "ui/ph_icon_beerbarrel.png";
	public override Model WorldModel => Model.Load( "models/clutter/barrel/wood_barrel.vmdl" );
	public override int SuiteItemCost => 500;
	public override ShopType ShopSeller => ShopType.Bar;

	int limitedUses = 20;

	public override void Interact( PHPawn player )
	{
		base.Interact( player );

		if ( IsClient )
			return;

		if ( player.TimeLastDrank < 0.5f )
			return;

		player.Drunkiness += 15.0f;
		player.TimeLastDrank = 0;

		if(player.Drunkiness >= 100.0f)
		{
			player.OnKilled();
			player.TimeLastDrank = 0;
		}

		limitedUses--;

		if ( limitedUses <= 0)
			Delete();
	}
}
