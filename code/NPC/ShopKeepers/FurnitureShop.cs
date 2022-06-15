using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;

public partial class FurnitureShop : ShopKeeperBase
{
	public override string NPCName => "Solid Furniture Store";

	public override void InteractWith( PHPawn player )
	{
		base.InteractWith( player );
	}
}

