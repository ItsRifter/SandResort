using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;

public partial class ClassicRadio : PCSuiteProps
{
	public override Model WorldModel => Model.Load( "models/citizen_props/beertankard01.vmdl" );

	public override void Interact( PCPawn player )
	{
		base.Interact( player );

		PlaySound( "interact_classicradio" );
	}
}

