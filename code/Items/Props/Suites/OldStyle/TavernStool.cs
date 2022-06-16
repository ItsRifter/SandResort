using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;

public partial class TavernStool : PHSittableProp
{
	public override string SuiteItemName => "Tavern Stool";
	public override string SuiteItemDesc => "A tavern stool for guests to sit";
	public override Model WorldModel => Model.Load( "models/furniture/tavern_stool/tavern_stool.vmdl" );
	public override int SuiteItemCost => 300;
	public override ShopType ShopSeller => ShopType.Furniture;
}
