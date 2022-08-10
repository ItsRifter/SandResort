using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;
using Sandbox.UI;

public class SCHud : RootPanel
{
	public static SCHud CurHud;
	public SCHud()
	{
		if( CurHud != null )
		{
			CurHud?.Delete();
			CurHud = null;
		}

		//TEMPORARY, we should make our own chatbox/scoreboard
		AddChild<ChatBox>();
		AddChild<Scoreboard<ScoreboardEntry>>();

		AddChild<CondoRecept>();

		CurHud = this;
	}
}

