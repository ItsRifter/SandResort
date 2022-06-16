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
public partial class SuiteRoomEnt : Entity
{
	public PHPawn SuiteOwner;

	[Property, FGDType("target_destination")]
	public string SuiteTeleporter { get; set; } = "";

	public SuiteTeleporter SuiteTele;

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
	}

	public override void ClientSpawn()
	{
		base.ClientSpawn();
	}

	public SuiteRoomEnt ClaimSuite(PHPawn player)
	{
		SuiteOwner = player;
		SuiteTele.ClaimedSuite = true;
		Log.Info( "Claimed" );

		return this;
	}

	public void RevokeSuite()
	{
		SuiteOwner = null;
	}

	public List<PHSuiteProps> SaveSuite()
	{
		var suiteProps = new List<PHSuiteProps>();

		foreach ( var item in FindInBox(WorldSpaceBounds) )
		{
			if ( item is PHSuiteProps prop )
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
