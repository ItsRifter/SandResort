using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;

public class AchData
{
	public string AchievementName { get; set; }
	public string AchievementClass { get; set; }
	public int AchievementProgress { get; set; }
	public bool IsCompleted { get; set; }
}

public interface IPlayerStat
{
	public string PlayerName { get; set; }
	public int PlayCoins { get; set; }
	public List<string> InventoryItems { get; set;  }
	public List<AchData> Achievements { get; set; }
}

public class PlayerData : IPlayerStat
{
	public string PlayerName { get; set; }
	public int PlayCoins { get; set; }
	public List<string> InventoryItems { get; set; }
	public List<AchData> Achievements { get; set; }
}

public partial class PHPawn
{
	public string PlayerName { get => Client.Name; }

	[Net, Local]
	public int PlayCoins { get; set; }

	public List<string> InventoryItems { get; }

	public void GiveCoins(int addAmt)
	{
		PlayCoins += addAmt;
	}

	public void TakeCoins(int subAmt)
	{
		PlayCoins -= subAmt;

		if(PlayCoins < 0) 
			PlayCoins = 0;
	}

	public void SetCoins(int setAmt)
	{
		PlayCoins = setAmt;
	}
}
