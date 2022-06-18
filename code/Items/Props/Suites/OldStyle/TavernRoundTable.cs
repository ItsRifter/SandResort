using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;

public partial class TavernRoundTable : PHSuiteProps
{
	public override string SuiteItemName => "Tavern Round Table";
	public override string SuiteItemDesc => "A round tavern table for serving drinks in the old times";
	public override string SuiteItemImage => "ui/ph_icon_tavernroundtable.png";
	public override Model WorldModel => Model.Load( "models/furniture/tavern_table/tavern_table_round.vmdl" );
	public override int SuiteItemCost => 350;
	public override ShopType ShopSeller => ShopType.Furniture;

}
