public class BarShop : ShopKeeperBase
{
	public override string NPCName => "Bartender";

	public override void InteractWith( LobbyPawn player )
	{
		base.InteractWith( player );
	}
}
