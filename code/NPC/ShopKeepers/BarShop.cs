using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;

public partial class BarShop : ShopKeeperBase
{
	public override string NPCName => "Bartender";

	public override void InteractWith( LobbyPawn player )
	{
		base.InteractWith( player );
	}
}
