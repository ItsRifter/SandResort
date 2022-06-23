public class NewGuest : AchBase
{
	public override string AchName => "New Guest Arrival";
	public override string AchDesc => "Start Playhome for the first time";
	public override int AchProgressMax => 1;
	public override int AchCoinsReward => 0;
	public override int AchProgress { get; set; } = 1;
}

