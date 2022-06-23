public class AdminNPC : ShopKeeperBase
{
	public override string NPCName => "Admin Test Shop";

	public override void InteractWith( LobbyPawn player )
	{
		base.InteractWith( player );
	}
}

