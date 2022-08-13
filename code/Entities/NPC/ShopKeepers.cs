using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;
using SandboxEditor;

[Library("sc_npc_condoreceptionist")]
[Title("Condo Receptionist"), Category("NPC")]
[EditorModel( "models/citizen/citizen.vmdl" )]
[HammerEntity]
partial class CondoReceptionist : AnimatedEntity, IUse
{
	TimeSince timeLastUse;

	[Net]
	Entity interacter { get; set; }

	CondoRecept condoPanel;

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

		condoPanel?.Delete();
		condoPanel = null;
	}

	[ClientRpc]
	public void CreateCondoPanel()
	{
		if ( condoPanel != null )
		{
			RemoveCondoPanel();
			return;
		}

		condoPanel = new CondoRecept();
		SCHud.CurHud.AddChild( condoPanel );
		
	}

	[ClientRpc]
	public void RemoveCondoPanel()
	{
		condoPanel?.Delete();
		condoPanel = null;
	}

	[Event.Frame]
	public void OnFrame()
	{
		if ( condoPanel == null )
			return;

		if(Position.Distance( interacter.Position ) > 90.0f )
		{
			RemoveCondoPanel();
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

		CreateCondoPanel( To.Single(user));

		timeLastUse = 0;

		return false;
	}
}
