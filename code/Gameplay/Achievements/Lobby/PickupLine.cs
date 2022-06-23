using System.Collections.Generic;
using Sandbox;

public class PickupLine : AchBase
{
	public override string AchName => "Best pickup line";
	public override string AchDesc => "Talk to the Suite Receptionist while drunk";
	public override int AchProgressMax => 1;
	public override int AchCoinsReward => 500;
	public override int AchProgress { get; set; }

	public override List<Entity> AchItemRewards => new List<Entity>
	{
		new BeerBottle()
	};
}
