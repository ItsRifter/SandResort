using Sandbox.UI;

public class PHHud : RootPanel
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

		//TEMPORARY, We should create our own chatbox with message storing n stuff
		AddChild<ChatBox>();

		AddChild<SuiteReceptionUI>();
		AddChild<Shop>();
		AddChild<Inventory>();
	}
}

