using Sandbox;

public class ClassicRadio : PHSuiteProps
{
	public override string SuiteItemName => "Classical Radio";
	public override string SuiteItemDesc => "An old time radio, only has one channel still available";
	public override string SuiteItemImage => "ui/ph_icon_classicradio.png";
	public override Model WorldModel => Model.Load( "models/radio/oldradio/oldradio.vmdl" );
	public override int SuiteItemCost => 250;
	public override ShopType ShopSeller => ShopType.Electric;

	Sound playingSound;

	public override void Interact( LobbyPawn player )
	{
		base.Interact( player );

		if ( IsClient )
			return;

		if ( playingSound.Finished )
		{
			int random = Rand.Int( 1, 2 );
			if( random == 1)
				playingSound = PlaySound( "interact_classicradio" );
			else if ( random == 2 )
				playingSound = PlaySound( "interact_classicradio_2" );
		}
		else if ( !playingSound.Finished )
		{
			playingSound.Stop();
		}
		
	}
}

