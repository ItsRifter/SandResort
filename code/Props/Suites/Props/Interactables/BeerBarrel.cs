using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;
public partial class BeerBarrel : PCSuiteProps
{
	public override Model WorldModel => Model.Load( "models/clutter/barrel/wood_barrel.vmdl" );

	int limitedUses = 20;
	TimeSince timeLastDrank;
	public override void Interact( PCPawn player )
	{
		base.Interact( player );

		if ( IsClient )
			return;

		if ( timeLastDrank < 0.5f )
			return;

		timeLastDrank = 0;
		limitedUses--;
		Log.Info( limitedUses );
		if ( limitedUses <= 0)
			Delete();
	}
}
