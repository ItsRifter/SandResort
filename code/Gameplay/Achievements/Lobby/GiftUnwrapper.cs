using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;
public partial class GiftUnwrapper : AchBase
{
	public override string AchName => "Gift Unwrapper";
	public override string AchDesc => "Unwrap 50 gifts";
	public override int AchProgressMax => 50;
	public override int AchCoinsReward => 1500;
	public override int AchProgress { get; set; }

}

