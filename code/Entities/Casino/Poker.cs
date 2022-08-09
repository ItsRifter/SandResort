using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;
using SandboxEditor;

[Library("sc_casino_poker")]
[Title("Poker Table"), Category("Casino"), Description("The poker table for players to play poker")]
[EditorModel( "models/citizen_props/crate01.vmdl" )]
[HammerEntity]

public class Poker : ModelEntity
{
	[Property, Title( "Bidding Requirement" ), Description( "How much do players need in chips to play" )]
	public int BiddingMinimum { get; set; } = 0;

	public List<LobbyPawn> CurPlayers;

	List<PHSittableProp> pokerChairs;

	public enum PokerState
	{
		Idle,
		PreFlop,
		Flop,
		Turn,
		River,
		Showdown,
	}

	public PokerState CurPokerState = PokerState.Idle;

	public override void Spawn()
	{
		base.Spawn();

		pokerChairs = new List<PHSittableProp>();
		CurPlayers = new List<LobbyPawn>();

		SetModel( "models/citizen_props/crate01.vmdl" );
		SetupPhysicsFromModel(PhysicsMotionType.Static);

		for ( int i = 0; i < 4; i++ )
		{
			PHSittableProp tableChair = new PHSittableProp();
			tableChair.SetParent( this );
			tableChair.SetModel( "models/furniture/tavern_stool/tavern_stool.vmdl" );
			tableChair.CanDirectlyInteract = false;

			//Probably temporary until someone makes a poker table model + chair
			switch (i)
			{
				case 0:
					tableChair.LocalPosition = new Vector3( 40, 0, 0 );
					tableChair.LocalRotation = Rotation.From( 0, 180, 0);
					break;

				case 1:
					tableChair.LocalPosition = new Vector3( -40, 0, 0 );
					tableChair.LocalRotation = Rotation.From( 0, 0, 0 );
					break;

				case 2:
					tableChair.LocalPosition = new Vector3( 0, 40, 0 );
					tableChair.LocalRotation = Rotation.From( 0, -90, 0 );
					break;

				case 3:
					tableChair.LocalPosition = new Vector3( 0, -40, 0 );
					tableChair.LocalRotation = Rotation.From( 0, 90, 0 );
					break;
			}

			pokerChairs.Add( tableChair );
		}
	}

	public void Interact(LobbyPawn player)
	{
		//If we're not idling, don't let the players join or leave mid game
		if ( CurPokerState != PokerState.Idle )
			return;

 		//If this player is joining, seat them and let them play
		if ( !CurPlayers.Contains(player) )
		{
			pokerChairs.First( x => x.SittingPlayer == null ).SitDown(player);
			CurPlayers.Add( player );
			return;
		}
	}

	public override void OnKilled()
	{
		foreach ( var child in Children )
			child.Delete();
		
		base.OnKilled();
	}
}

