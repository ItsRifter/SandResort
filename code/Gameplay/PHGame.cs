
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

			AdminList = new List<long>()
			{
				76561197972285500, // ItsRifter
				76561198320469005, // RedHacker2 / Self Proclaimed God,
				76561197991940798, // Baik
				76561197975823217, // G Kaf
				76561198169265681, // Jake
				76561198142761957 // Lokiv
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
			AdminList = new List<long>()
			{
				76561197972285500, // ItsRifter
				76561198320469005, // RedHacker2 / Self Proclaimed God,
				76561197991940798, // Baik
				76561197975823217, // G Kaf
				76561198169265681, // Jake
				76561198142761957 // Lokiv
			};

			ResetSuitePropsList();
		}

		if ( IsClient )
			new PHHud();
	}

	public List<(string, string, int, PHSuiteProps.ShopType)> GrabSuiteItem( string itemToGrab )
	{
		var grabbedItem = new List<(string, string, int, PHSuiteProps.ShopType)>();
		var prop = TypeLibrary.Create<PHSuiteProps>( itemToGrab );

		if ( prop == null )
			return null;

		grabbedItem.Add((prop.SuiteItemName, prop.SuiteItemDesc, prop.SuiteItemCost, prop.ShopSeller));

		prop.Delete();

		return grabbedItem;
	}

	public IList<string> GetAllSuiteProps()
	{
		return AllSuiteProps;
	}

	public void ResetSuitePropsList()
	{
		if ( AllSuiteProps.Count > 0 )
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
		if ( AdminList == null || !AdminList.Contains( client.PlayerId ) )
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
		if ( AdminList == null || !AdminList.Contains( player.PlayerId ) )
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

	public override void PostLevelLoaded()
	{
		base.PostLevelLoaded();
	}

	public override void ClientJoined( Client client )
	{
		base.ClientJoined( client );

		var pawn = new PHPawn();
		pawn.Spawn();

		client.Pawn = pawn;

		if ( !LoadSave( client ) )
			NewPlayer( client );

	}
	public override void ClientDisconnect( Client cl, NetworkDisconnectionReason reason )
	{
		CommitSave( cl );

		if ( cl.Pawn is PHPawn player )
		{
			if( player.CurSuite != null )
			{
				player.CurSuite.RevokeSuite( player );
				Log.Info( $"{cl.Name} was automatically checked out by disconnecting" );
			}

			if(player.SitProp != null)
			{
				Log.Info( player.SitProp );
			}
		}

		base.ClientDisconnect( cl, reason );
	}

	public override void Shutdown()
	{
		foreach ( Client cl in Client.All)
		{
			CommitSave( cl );
		}

		base.Shutdown();
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
