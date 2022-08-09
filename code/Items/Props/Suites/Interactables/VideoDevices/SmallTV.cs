using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;

public partial class SmallTV : PHSuiteProps
{
	public override string SuiteItemName => "Small TV";
	public override string SuiteItemDesc => "A small and simple televsion for your viewing pleasure";
	public override string SuiteItemImage => "ui/sc_icon_tv_small.png";
	public override Model WorldModel => Model.Load( "models/television/smalltv/smalltv.vmdl" );
	public override int SuiteItemCost => 425;
	public override ShopType ShopSeller => ShopType.Electric;

}

