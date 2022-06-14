
using Sandbox;
using Sandbox.UI.Construct;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

public partial class PCGame : Game
{
	public static PCGame Instance { get; private set; } = Current as PCGame;

	PCHud curHud;
	public PCGame()
	{
		if(IsServer)
		{
			AdminList = new string[]
			{
				"ItsRifter",
				"Pixel³",
				"Baik"
			};
		}

		if(IsClient)
		{
			curHud = new PCHud();
		}
	}

	[Event.Hotload]
	public void Hotload()
	{
		if ( IsServer )
		{
			AdminList = new string[]
			{
				"ItsRifter",
				"Pixel³",
				"Baik"
			};
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

		var pawn = new PCPawn();
		pawn.Spawn();

		client.Pawn = pawn;
	}
	public override void ClientDisconnect( Client cl, NetworkDisconnectionReason reason )
	{
		base.ClientDisconnect( cl, reason );
	}

	public override void DoPlayerSuicide( Client cl )
	{
		if(cl.Pawn is PCPawn player)
		{
			if( player.timeLastRespawn >= 3.0f)
				base.DoPlayerSuicide( cl );
		}
	}
}
