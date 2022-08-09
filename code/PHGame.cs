
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

	public PHGame()
	{
		if(IsServer)
		{
			
		}

		if ( IsClient )
		{

		}
	}

	[Event.Hotload]
	public void Hotload()
	{
		
	}

	//Allow admins to use dev cam
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

	//Allow admins to toggle noclipping
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
	}
	public override void ClientDisconnect( Client cl, NetworkDisconnectionReason reason )
	{
		base.ClientDisconnect( cl, reason );
	}

	public override void Shutdown()
	{	
		base.Shutdown();
	}

	public override void DoPlayerSuicide( Client cl )
	{
		base.DoPlayerSuicide(cl);
	}
}
