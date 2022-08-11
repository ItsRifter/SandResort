﻿
using Sandbox;
using Sandbox.UI.Construct;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

public partial class SCGame : Game
{
	public static SCGame Instance { get; protected set; } = Current as SCGame;

	public SCGame()
	{
		if(IsServer)
		{

		}

		if ( IsClient )
		{
			_ = new SCHud();
		}
	}

	[Event.Hotload]
	public void Hotload()
	{
		if ( IsClient )
		{
			_ = new SCHud();
		}
	}

	public override void DoPlayerDevCam( Client client )
	{
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

	public override void PostLevelLoaded()
	{
		base.PostLevelLoaded();
	}

	public override void ClientJoined( Client client )
	{
		base.ClientJoined( client );

		var pawn = new BasePawn();

		pawn.Spawn();

		client.Pawn = pawn;

		if ( !LoadPlayer( client ) )
			SavePlayer( client );

	}
	public override void ClientDisconnect( Client cl, NetworkDisconnectionReason reason )
	{
		SavePlayer( cl );

		base.ClientDisconnect( cl, reason );
	}

	public override void Shutdown()
	{
		foreach ( Client cl in Client.All )
			SavePlayer( cl );

		base.Shutdown();
	}

	public override void DoPlayerSuicide( Client cl )
	{
		base.DoPlayerSuicide(cl);
	}
}
