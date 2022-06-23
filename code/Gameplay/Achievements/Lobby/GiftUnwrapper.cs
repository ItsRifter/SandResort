public class GiftUnwrapper : AchBase
{
	public override string AchName => "Gift Unwrapper";
	public override string AchDesc => "Unwrap 50 gifts";
	public override int AchProgressMax => 50;
	public override int AchCoinsReward => 1500;
	public override int AchProgress { get; set; }

}

