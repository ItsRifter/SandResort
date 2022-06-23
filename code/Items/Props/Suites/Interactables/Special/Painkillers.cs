using Sandbox;

public class Painkillers : PHSuiteProps
{
	public override string SuiteItemName => "Painkiller Bottle";
	public override string SuiteItemDesc => "Relives you of pain";
	public override string SuiteItemImage => "ui/ph_icon_painkillers.png";
	public override Model WorldModel => Model.Load( "models/special/painkillers/painkillers.vmdl" );
	public override int SuiteItemCost => 75;
	public override ShopType ShopSeller => ShopType.Bar;

	public override void Interact( LobbyPawn player )
	{
		base.Interact( player );
		
		Particles.Create( "particles/explosion/pills_explosion.vpcf", Position );
		
		if ( IsClient )
			return;

		Delete();

	}
}

