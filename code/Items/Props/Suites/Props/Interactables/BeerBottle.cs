using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;
public partial class BeerBottle : PHSuiteProps
{
	public override Model WorldModel => Model.Load( "models/clutter/glassbottle/glassbottle.vmdl" );

	public override int SuiteItemCost => 25;

	public override void Interact( PHPawn player )
	{
		base.Interact( player );

		Delete();
	}
}
