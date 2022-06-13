
using Sandbox;
using Sandbox.UI.Construct;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

public partial class PCGame : Game
{
	public PCGame()
	{

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
