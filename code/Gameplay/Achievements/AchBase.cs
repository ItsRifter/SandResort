using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;

public class AchBase
{
	public virtual string AchName => "Base Achievement";
	public virtual string AchDesc => "[Insert Text here]";
	public virtual string AchIcon => "";
	public virtual bool IsSecret => false;
	public virtual int AchGoal => 1;
	public virtual int MoneyReward => 1;

	public int Progress { get; set; }

	public bool IsCompleted { get; set; }

	//TODO: Item rewards
}

