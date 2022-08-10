using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;
using SandboxEditor;

public class ShopKeeperBase : AnimatedEntity, IUse
{
	TimeSince timeNextUse;

	public override void Spawn()
	{
		base.Spawn();

		SetModel( "models/citizen/citizen.vmdl" );
	}
	public bool IsUsable( Entity user )
	{
		return true;
	}

	public bool OnUse( Entity user )
	{
		if ( timeNextUse < 0.3f )
			return false;

		if ( user is not BasePawn player )
			return false;



		timeNextUse = 0;

		return true;
	}
}
