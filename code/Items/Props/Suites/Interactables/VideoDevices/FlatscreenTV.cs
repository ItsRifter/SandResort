using Sandbox;

public class FlatscreenTV : PHSuiteProps
{
	public override string SuiteItemName => "Small Flatscreen TV";
	public override string SuiteItemDesc => "A small flatscreen TV to play whatever video you like";
	public override string SuiteItemImage => "ui/ph_icon_tv_flatscreen.png";
	public override Model WorldModel => Model.Load( "models/television/flatscreen/flatscreen_tv.vmdl" );
	public override int SuiteItemCost => 1150;
	public override ShopType ShopSeller => ShopType.Electric;

}

