using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;

public class AfterLife : AchBase
{
	public override string AchName => "After-After Life";
	public override string AchDesc => "Commit suicide [int] times";
	public override string AchIcon => "";
	public override bool IsSecret => false;
	public override int AchGoal => 2;
	public override int MoneyReward => 250;
}

public class Walkathon : AchBase
{
	public override string AchName => "Walkathon";
	public override string AchDesc => "Walk a total of [int] times";
	public override string AchIcon => "";
	public override bool IsSecret => false;
	public override int AchGoal => 100;
	public override int MoneyReward => 250;
}
