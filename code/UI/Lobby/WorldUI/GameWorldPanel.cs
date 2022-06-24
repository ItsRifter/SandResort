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

	public string GameName = "Missing Name";
	public string GameDescription = "Missing Description";

	public Panel SubGamePanel;
	public Panel SubGameBGImage;

	public Panel SubGameStatusText_waiting;
	public Panel SubGameStatusText_voting;
	public Panel SubGameStatusText_starting;

	public Label PlayerStatus_QueuePlayerCount;

	public Panel GameStatus_Waiting;
	public Panel GameStatus_InGame;

	public GameWorldPanel()
	{
		PanelBounds = new Rect(-500, -500, 5000, 3000);

		StyleSheet.Load( "UI/Styles/Lobby/WorldUI/GameWorldPanel.scss" );

		PlayerInQueue = new List<string>();
		PlayerPanels = new List<Label>();

		SubGamePanel = Add.Panel("subgame");
		SubGamePanel.SetClass( "colorsGreen", true );
		SubGamePanel.SetClass( "colorsBlue", false );
		
		// HEADER PANEL
		Panel subGamePanelHeader = SubGamePanel.Add.Panel("subgame-header");
		// HEADER
		SubGameBGImage = subGamePanelHeader.Add.Panel( "subgame-img" );
		SubGameBGImage.Add.Label( GameName , "gametitle");

		// HEADER STATUS
		Panel startingStatus = subGamePanelHeader.Add.Panel( "starting-status" );
		SubGameStatusText_waiting = startingStatus.Add.Panel("status statusCurrent");
		SubGameStatusText_waiting.Add.Label( "Waiting", "text");
		SubGameStatusText_voting = startingStatus.Add.Panel( "status" );
		SubGameStatusText_voting.Add.Label( "Voting", "text" );
		SubGameStatusText_starting = startingStatus.Add.Panel( "status" );
		SubGameStatusText_starting.Add.Label( "Starting", "text" );

		// QUEUE/STATUS PANEL
		Panel subGamePanelStatus = SubGamePanel.Add.Panel("subgame-status");

		// QUEUE
		Panel Queue = subGamePanelStatus.Add.Panel("queue");
		Panel PlayersStatus = Queue.Add.Panel("players-status");
		PlayersStatus.Add.Label( "Queue", "text" );
		Label PlayersStatus_queuePlayerCount = PlayersStatus.Add.Label( "??/?? Players", "text" );
		Panel PlayersList = Queue.Add.Panel("players");

		for( int i = 0; i < 10; i++ )
		{
			Panel user = Add.Panel( "player" );
			Panel pfp = user.Add.Panel( "pfp" );
			user.Add.Label( $"player {i}", "name" );

			pfp.Style.BackgroundImage = Texture.Load( "ui/alex.jpg" );

			PlayersList.AddChild( user );
		}
		// GAME STATUS
		Panel GameStatus = subGamePanelStatus.Add.Panel("gameStatus");

		// NOT IN GAME STATUS
		GameStatus_Waiting = GameStatus.Add.Panel("GameInfo");
		GameStatus_Waiting.Add.Label("Description", "text");
		GameStatus_Waiting.Add.Label( GameDescription , "text");
		// IN GAME STATUS
		GameStatus_InGame = GameStatus.Add.Panel("inGame");

		Interacted = false;
	}

	public override void Tick()
	{
		base.Tick();

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
