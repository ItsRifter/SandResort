using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;
public partial class Jetpack : PHUsableItemBase
{
	public override Model ItemModel => Model.Load( "models/jetpack/jetpack/jetpack.vmdl" );

	public float JetFuel;

	float JetFuelMax = 125.0f;
	float JetFuelUsage = 2.5f;

	float timeUntilRefuel = 3.5f;

	TimeSince timeLastUse;

	public override void Spawn()
	{
		base.Spawn();
	}

	public override void Simulate( Client cl )
	{
		base.Simulate( cl );

		if( timeLastUse >= timeUntilRefuel && JetFuel < JetFuelMax )
		{
			JetFuel += 0.5f;

			if ( JetFuel > JetFuelMax )
				JetFuel = 50.0f;
		}

		if (Input.Down(InputButton.Jump) && JetFuel > 0 && (Parent as PHPawn).Controller is not NoclipController)
		{
			Parent.Velocity += Vector3.Up * 15f;

			timeLastUse = 0;
			JetFuel -= 2.5f;

			if ( JetFuel < 0 )
				JetFuel = 0;
		}
	}
}

