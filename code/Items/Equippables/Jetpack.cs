using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;
public partial class Jetpack : PHUsableItemBase
{
	public override Model ItemModel => Model.Load( "models/jetpack/jetpack/jetpack.vmdl" );

	public float JetFuel = 50.0f;
	float JetFuelUsage = 2.5f;

	float timeUntilRefuel = 5.0f;

	TimeSince timeLastUse;

	public override void Spawn()
	{
		base.Spawn();
	}

	public override void Simulate( Client cl )
	{
		base.Simulate( cl );

		if( timeLastUse >= timeUntilRefuel && JetFuel < 50.0f )
		{
			JetFuel += 0.5f;

			if ( JetFuel > 50.0f )
				JetFuel = 50.0f;
		}

		if(Input.Down(InputButton.Jump))
		{
			Owner.Velocity += Vector3.Up * 2.5f;

			timeLastUse = 0;
			JetFuel -= 2.5f;
		}
	}
}

