using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;
using SandboxEditor;

[Library("ph_suite_room")]
[Title("Suite Room"), Description("Defines a suite room for saving and loading player owner items")]
[HammerEntity]
[SupportsSolid]
public partial class SuiteRoomEnt : BaseTrigger
{
	[Net]
	public LobbyPawn SuiteOwner { get; set; }

	public bool IsLocked = false;
	public bool ClaimedSuite = false;

	[Property, FGDType( "target_destination" )]
	public string SuiteKickerDestination { get; set; } = "";

	public TeleDest SuiteKickedDest;

	[Property, FGDType( "target_destination" )]
	public string SuiteTeleporterEntity { get; set; } = "";

	public PHTriggerTeleport SuiteTeleporter;

	public override void Spawn()
	{
		base.Spawn();

		Transmit = TransmitType.Default;
	}

	[Event.Tick.Server]
	public void FindSuiteTeleport()
	{
		if ( SuiteKickedDest == null && !string.IsNullOrEmpty( SuiteKickerDestination ) )
			SuiteKickedDest = Entity.FindByName( SuiteKickerDestination ) as TeleDest;

		if ( SuiteTeleporter == null && !string.IsNullOrEmpty( SuiteTeleporterEntity ) )
		{
			SuiteTeleporter = Entity.FindByName( SuiteTeleporterEntity ) as PHTriggerTeleport;
			SuiteTeleporter.Disable();
		}
	}

	public override void ClientSpawn()
	{
		base.ClientSpawn();
	}

	public void RevokeSuite(LobbyPawn player)
	{
		player.CurSuite.KickGuest();
		player.CurSuite.SuiteOwner = null;
		player.CurSuite = null;
		SuiteTeleporter.Disable();

		PHGame.Instance.CommitSave( player.Client, SaveSuite() );
	}

	public void KickGuest(LobbyPawn player = null)
	{
		if(player == null)
		{
			foreach ( var guest in FindInBox(WorldSpaceBounds) )
			{
				if(guest is LobbyPawn guestPlayer)
				{
					using ( Prediction.Off() )
					{
						guestPlayer.Position = SuiteKickedDest.Position;
						guestPlayer.Rotation = SuiteKickedDest.Rotation;
					}
				}
			}
		}
		else
		{
			using ( Prediction.Off() )
			{
				player.Position = SuiteKickedDest.Position;
				player.Rotation = SuiteKickedDest.Rotation;
			}
		}
	}

	public List<PHSuiteProps> SaveSuite()
	{
		var suiteProps = new List<PHSuiteProps>();

		foreach ( var item in FindInBox(WorldSpaceBounds) )
		{
			if ( item is PHSuiteProps prop && prop.PropOwner != null )
				suiteProps.Add( prop );
		}

		return suiteProps;
	}

	public void LoadSuite(List<SuitePropInfo> fileProps)
	{
		foreach ( var item in fileProps )
		{
			var suiteProp = new PHSuiteProps();
			suiteProp.SetParent( this );
			suiteProp.Model = item.Model;
			suiteProp.LocalPosition = item.Pos;
			suiteProp.LocalRotation = item.Rot;
		}
	}

}
