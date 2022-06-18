using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;
public partial class OCD : AchBase
{
	public override string AchName => "Suite OCD";
	public override string AchDesc => "Place items in your suite 150 times";
	public override int AchProgressMax => 150;
	public override int AchCoinsReward => 5000;
	public override int AchProgress { get; set; }

}

