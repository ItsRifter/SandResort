using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;
using SandboxEditor;

[Library("sc_npc_shopkeeper")]
[Title("Shopkeeper"), Category("NPC")]
[EditorModel( "models/citizen/citizen.vmdl" )]
[HammerEntity]
partial class ShopKeepers : AnimatedEntity, IUse
{
	TimeSince timeLastUse;

	[Net]
	Entity interacter { get; set; }

	ShopUI shopPanel;
	public override void Spawn()
	{
		timeLastUse = 0;
		SetModel( "models/citizen/citizen.vmdl" );
		SetupPhysicsFromModel( PhysicsMotionType.Keyframed );

		Transmit = TransmitType.Always;

		Tags.Add( "shop" );
	}

	public override void ClientSpawn()
	{
		
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();

		shopPanel?.Delete();
		shopPanel = null;
	}

	[ClientRpc]
	public void CreateShopPanel()
	{
		if ( shopPanel != null )
		{
			RemoveShopPanel();
			return;
		}
		shopPanel = new ShopUI();
		SCHud.CurHud.AddChild( shopPanel );
		
	}

	[ClientRpc]
	public void RemoveShopPanel()
	{
		shopPanel?.Delete();
		shopPanel = null;
	}

	[Event.Frame]
	public void OnFrame()
	{
		if ( shopPanel == null )
			return;

		if(Position.Distance( interacter.Position ) > 90.0f )
		{
			RemoveShopPanel();
		}
	}

	public bool IsUsable( Entity user )
	{
		return timeLastUse > 0.5f;
	}

	public bool OnUse( Entity user )
	{
		if ( !IsUsable(user) )
			return false;

		interacter = user;

		CreateShopPanel(To.Single(user));

		timeLastUse = 0;

		return false;
	}
}
