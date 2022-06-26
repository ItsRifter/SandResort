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
	public List<string> PlayerInQueue;

	public Ray Raycast;
	public bool HasInteracted;
	public Client PlayerClient;

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
	public Panel PlayerList;
	public GameWorldPanel()
	{
		PanelBounds = new Rect(-500, -500, 3500, 2000);

		StyleSheet.Load( "UI/Styles/Lobby/WorldUI/GameWorldPanel.scss" );

		PlayerInQueue = new List<string>();

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
		PlayerList = Queue.Add.Panel("players");

		/*for( int i = 0; i < 10; i++ )
		{
			Panel user = Add.Panel( "player" );
			Panel pfp = user.Add.Panel( "pfp" );
			user.Add.Label( $"player {i}", "name" );

			pfp.Style.BackgroundImage = Texture.Load( "ui/alex.jpg" );

			PlayersList.AddChild( user );
		}*/

		// GAME STATUS
		Panel GameStatus = subGamePanelStatus.Add.Panel("gameStatus");

		// NOT IN GAME STATUS
		GameStatus_Waiting = GameStatus.Add.Panel("GameInfo");
		Panel panel_description = GameStatus_Waiting.Add.Panel( "desc" );
		panel_description.Add.Label("Description", "text");
		panel_description.Add.Label( GameDescription , "text");
		Panel GameStatus_Waiting_player = GameStatus_Waiting.Add.Panel( "countdown" );
		GameStatus_Waiting_player.Add.Panel( "bar" );
		Label GameStatus_barText = GameStatus_Waiting_player.Add.Label("??? Players required to start", "barText");
		// IN GAME STATUS
		GameStatus_InGame = GameStatus.Add.Panel("inGame");

		HasInteracted = false;
	}

	public override void Tick()
	{
		base.Tick();

		if(HasInteracted && PlayerClient != null)
		{
			UpdateQueue( PlayerClient );
		}
	}

	public void UpdateQueue(Client client)
	{
		if ( PlayerInQueue.Contains( client.Name ) )
		{
			PlayerInQueue.Remove( client.Name );

			foreach ( var panel in PlayerList.Children )
			{
				if ( panel is Label label && label.Text.Contains(client.Name) )
				{
					Log.Info( $"{client.Name} removed from queue" );
					panel.Delete();
					break;
				}
			}
		}
		else
		{
			Panel player = Add.Panel( "player" );
			Panel pfp = player.Add.Panel( "pfp" );
			Label user = player.Add.Label( client.Name , "name" );

			pfp.Style.BackgroundImage = Texture.Load( "ui/alex.jpg" );

			PlayerList.AddChild( player );

			PlayerInQueue.Add( client.Name );
			Log.Info( $"{client.Name} added to queue" );
		}
	}
}
