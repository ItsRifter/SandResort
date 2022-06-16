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

	float jetFuelMax = 125.0f;
	float jetFuelUsage = 2.5f;

	float timeUntilRefuel = 3.5f;

	TimeSince timeLastUse;

	public override void Spawn()
	{
		base.Spawn();
	}

	public override void Simulate( Client cl )
	{
		base.Simulate( cl );

		if( timeLastUse >= timeUntilRefuel && JetFuel < jetFuelMax )
		{
			JetFuel += 0.5f;

			if ( JetFuel > jetFuelMax )
				JetFuel = 50.0f;
		}

		if (Input.Down(InputButton.Jump) && JetFuel > 0 && (Parent as PHPawn).Controller is not NoclipController)
		{
			Parent.Velocity += Vector3.Up * 15f;

			timeLastUse = 0;
			JetFuel -= jetFuelUsage;

			if ( JetFuel < 0 )
				JetFuel = 0;
		}
	}
}

