﻿
using Sandbox;
using Sandbox.UI.Construct;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

public partial class PHGame : Game
{
	public static PHGame Instance { get; private set; } = Current as PHGame;

	[Net]
	public IList<string> AllSuiteProps { get; protected set; }

	List<string> skipPropItems;

	public PHGame()
	{
		if(IsServer)
		{
			AllSuiteProps = new List<string>();

			AdminList = new List<string>()
			{
				"ItsRifter",
				"Self Proclaimed God",
				"Baik"
			};

			skipPropItems = new List<string>() 
			{
				"PHSuiteProps",
				"VideoAudioPlayer"
			};

			ResetSuitePropsList();
		}

		if ( IsClient )
			new PHHud();
	}

	[Event.Hotload]
	public void Hotload()
	{
		if ( IsServer )
		{
			AdminList = new List<string>()
			{
				"ItsRifter",
				"Self Proclaimed God",
				"Baik"
			};

			ResetSuitePropsList();
		}

		if ( IsClient )
			new PHHud();
	}

	public IList<string> GetAllSuiteProps()
	{
		return AllSuiteProps;
	}

	public void ResetSuitePropsList()
	{
		if( AllSuiteProps.Count > 0 )
			AllSuiteProps.Clear();

		foreach ( var item in TypeLibrary.GetTypes<PHSuiteProps>())
		{
			if ( skipPropItems.Contains(item.FullName) )
				continue;

			if ( AllSuiteProps.Contains(item.FullName) )
				continue;

			AllSuiteProps.Add(item.FullName);
		}
	}

	public override void DoPlayerDevCam( Client client )
	{
		if ( AdminList == null || !AdminList.Contains( client.Name ) )
			return;

		var camera = client.Components.Get<DevCamera>( true );

		if ( camera == null )
		{
			camera = new DevCamera();
			client.Components.Add( camera );
			return;
		}

		camera.Enabled = !camera.Enabled;
	}

	public override void DoPlayerNoclip( Client player )
	{
		if ( AdminList == null || !AdminList.Contains( player.Name ) )
			return;

		if ( player.Pawn is Player basePlayer )
		{
			if ( basePlayer.DevController is NoclipController )
			{
				Log.Info( player.Name + " Noclip Mode Off" );
				basePlayer.DevController = null;
			}
			else
			{
				Log.Info( player.Name + " Noclip Mode On" );
				basePlayer.DevController = new NoclipController();
			}
		}
	}

	public override void ClientJoined( Client client )
	{
		base.ClientJoined( client );

		var pawn = new PHPawn();
		pawn.Spawn();

		client.Pawn = pawn;
	}
	public override void ClientDisconnect( Client cl, NetworkDisconnectionReason reason )
	{
		base.ClientDisconnect( cl, reason );
	}

	public override void DoPlayerSuicide( Client cl )
	{
		if(cl.Pawn is PHPawn player)
		{
			if( player.timeLastRespawn >= 3.0f)
				base.DoPlayerSuicide( cl );
		}
	}
}
