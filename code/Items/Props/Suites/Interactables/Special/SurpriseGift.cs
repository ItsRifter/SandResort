using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;

public partial class SurpriseGift : PHSuiteProps
{
	public override string SuiteItemName => "Surprise Gift";
	public override string SuiteItemDesc => "A mysterious gift containing a random item, wonder what's inside";
	public override string SuiteItemImage => "ui/sc_icon_gift.png";
	public override Model WorldModel => Model.Load( "models/special/gift/gift.vmdl" );
	public override int SuiteItemCost => 100;
	public override ShopType ShopSeller => ShopType.Bar;

	public override void Interact( LobbyPawn player )
	{
		base.Interact( player );

		if ( IsClient )
			return;

		Particles.Create( "particles/confetti/confetti_splash.vpcf", Position );

		player.CheckOrUpdateAchievement( "Gift Unwrapper", "GiftUnwrapper" );

		Sound.FromWorld( "gift_reveal", Position);

		//Test for now
		var surpriseItem = new BeerBottle();
		surpriseItem.Position = Position;
		surpriseItem.Rotation = Rotation;
		surpriseItem.PropOwner = player;

		Delete();

		surpriseItem.Spawn();
	}
}

