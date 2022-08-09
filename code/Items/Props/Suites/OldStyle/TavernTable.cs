using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;

public partial class TavernTable : PHSuiteProps
{
	public override string SuiteItemName => "Tavern Table";
	public override string SuiteItemDesc => "A tavern table for serving drinks in the old times";
	public override string SuiteItemImage => "ui/sc_icon_taverntable.png";
	public override Model WorldModel => Model.Load( "models/furniture/tavern_table/tavern_table.vmdl" );
	public override int SuiteItemCost => 350;
	public override ShopType ShopSeller => ShopType.Furniture;

}
