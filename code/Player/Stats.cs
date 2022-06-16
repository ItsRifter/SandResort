using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;

public interface IPlayerStat
{
	public string PlayerName { get; }
	public int PlayCoins { get; set; }
}

public partial class PHPawn : IPlayerStat
{
	public string PlayerName { get => Client.Name; }

	[Net, Local]
	public int PlayCoins { get; set; }

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
