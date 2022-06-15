using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

public partial class PHHud : RootPanel
{
	public static PHHud Current;

	public PHHud()
	{
		if(Current != null)
		{
			Current?.Delete();
			Current = null;
		}

		Current = this;

		//TEMPORARY, We should create our own scoreboard
		AddChild<Scoreboard<ScoreboardEntry>>();

		AddChild<HudTracker>();

		AddChild<ChatBox>();
		AddChild<Shop>();
	}
}

