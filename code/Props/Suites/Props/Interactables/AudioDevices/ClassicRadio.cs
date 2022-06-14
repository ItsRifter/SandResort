using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;

public partial class ClassicRadio : PCSuiteProps
{
	public override Model WorldModel => Model.Load( "models/clutter/glassbottle/glassbottle.vmdl" );

	Sound playingSound;

	public override void Interact( PCPawn player )
	{
		base.Interact( player );

		if( playingSound.Finished )
			playingSound = PlaySound( "interact_classicradio" );
	}
}

