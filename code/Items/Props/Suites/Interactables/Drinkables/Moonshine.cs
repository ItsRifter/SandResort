using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;
public partial class Moonshine : PHSuiteProps
{
	public override string SuiteItemName => "Moonshine Bottle";
	public override string SuiteItemDesc => "A rather big bottle containing alcohol, seriously drink very responibly";
	public override Model WorldModel => Model.Load( "models/clutter/moonshine/moonshine.vmdl" );
	public override int SuiteItemCost => 50;
	public override ShopType ShopSeller => ShopType.Bar;

	public override void Interact( PHPawn player )
	{
		base.Interact( player );

		player.Drunkiness += 20.0f;
		player.TimeLastDrank = 0;

		if ( player.Drunkiness >= 100.0f )
		{
			player.OnKilled();
			player.TimeLastDrank = 0;
		}

		Delete();
	}
}
