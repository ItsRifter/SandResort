using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;

public partial class JumpingJacks : AchBase
{
	public override string AchName => "Jumping Jacks";
	public override string AchDesc => "Jump a total of 50,000 times";
	public override int AchProgressMax => 50000;
	public override int AchCoinsReward => 10000;
	public override int AchProgress { get; set; }

}
