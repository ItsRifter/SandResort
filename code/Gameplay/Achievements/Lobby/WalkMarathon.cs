using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;

public partial class WalkMarathon : AchBase
{
	public override string AchName => "Walk Marathon";
	public override string AchDesc => "Walk a total of 100,000 steps";
	public override int AchProgressMax => 100000;
	public override int AchCoinsReward => 12500;
}

