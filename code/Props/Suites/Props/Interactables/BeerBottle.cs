using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;
public partial class BeerBottle : PCSuiteProps
{
	public override Model WorldModel => Model.Load( "models/clutter/glassbottle/glassbottle.vmdl" );

	public override void Interact( PCPawn player )
	{
		base.Interact( player );

		Delete();
	}
}
