using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

public partial class GameWorldPanel : WorldPanel
{
	public bool Interacted;

	public List<string> PlayerInQueue;

	public List<Label> PlayerPanels;

	public TraceResult RayResult;

	public BBox TriggerBox;

	public GameWorldPanel()
	{
		StyleSheet.Load( "UI/Styles/Lobby/WorldUI/GameWorldPanel.scss" );

		PlayerInQueue = new List<string>();
		PlayerPanels = new List<Label>();

		Add.Label( "Test SubGame Panel" );

		Interacted = false;
	}

	public override void Tick()
	{
		base.Tick();

		DebugOverlay.Box(TriggerBox.Mins, TriggerBox.Maxs);



	}

	public void UpdateQueue(string playerName)
	{
		if ( PlayerInQueue == null)
			PlayerInQueue = new List<string>();

		if( PlayerPanels == null )
			PlayerPanels = new List<Label>();

		if ( PlayerInQueue.Contains( playerName ) )
		{
			Label newPlayer = Add.Label( playerName );

			PlayerInQueue.Remove( playerName );

			PlayerPanels.Add(newPlayer);
		}
		else
		{
			PlayerInQueue.Add( playerName );
			PlayerPanels.Find(x => x.Text == playerName).Delete();
		}

	}
}
