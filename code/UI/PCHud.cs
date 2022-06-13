﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

public partial class PCHud : RootPanel
{
	public PCHud()
	{
		//TEMPORARY, We should create our own scoreboard
		AddChild<Scoreboard<ScoreboardEntry>>();

		AddChild<CoinTracker>();

		AddChild<ChatBox>();
	}
}

