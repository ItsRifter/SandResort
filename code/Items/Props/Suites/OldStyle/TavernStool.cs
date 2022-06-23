using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;

public partial class TavernStool : PHSittableProp
{
	public override Vector3 SitLocalPos => new Vector3( 0, 0, 7 );
	public override string SuiteItemName => "Tavern Stool";
	public override string SuiteItemDesc => "A tavern stool for guests to sit";
	public override string SuiteItemImage => "ui/ph_icon_tavernstool.png";
	public override Model WorldModel => Model.Load( "models/furniture/tavern_stool/tavern_stool.vmdl" );
	public override int SuiteItemCost => 300;
	public override ShopType ShopSeller => ShopType.Furniture;
}
