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
	public PHPawn SuiteOwner;

	[Property, FGDType("target_destination")]
	public string SuiteTeleporter { get; set; } = "";

	[Property, FGDType( "target_destination" )]
	public string SuiteKickerDestination { get; set; } = "";

	public SuiteTeleporter SuiteTele;
	public TeleDest SuiteKickedDest;

	public override void Spawn()
	{
		base.Spawn();
	}

	[Event.Tick.Server]
	public void FindSuiteTeleport()
	{
		if( SuiteTele == null)
		{
			foreach ( var tele in All.OfType<SuiteTeleporter>() )
			{
				if ( tele.Name.Contains( SuiteTeleporter ) )
					SuiteTele = tele;
			}
		}

		if ( SuiteKickedDest == null )
		{
			foreach ( var tele in All.OfType<TeleDest>() )
			{
				if ( tele.Name.Contains( SuiteKickerDestination ) )
					SuiteKickedDest = tele;
			}
		}
	}

	public override void ClientSpawn()
	{
		base.ClientSpawn();
	}

	public void RevokeSuite(PHPawn player)
	{
		var test = SaveSuite();

		player.CurSuite.KickGuest();
		player.CurSuite.SuiteTele.ClaimedSuite = false;
		player.CurSuite.SuiteOwner = null;
		player.CurSuite = null;

		PHGame.Instance.CommitSave( player.Client, SaveSuite() );
	}

	public void KickGuest(PHPawn player = null)
	{
		if(player == null)
		{
			foreach ( var guest in FindInBox(WorldSpaceBounds) )
			{
				if(guest is PHPawn guestPlayer)
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
