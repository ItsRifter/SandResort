using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;

public partial class PHPawn
{
	[Net]
	public int PlayCoins { get; protected set; }

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
}
