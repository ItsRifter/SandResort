using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;
public partial class NewGuest : AchBase
{
	public override string AchName => "New Guest Arrival";
	public override string AchDesc => "Start Playhome for the first time";
	public override int AchProgressMax => 1;
	public override int AchCoinsReward => 0;
}

