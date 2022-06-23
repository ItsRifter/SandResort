using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;

public partial class BarShop : ShopKeeperBase
{
	public override string NPCName => "Bartender";
	public override List<string> ClothingModels => new List<string>
	{
		"models/citizen_clothes/jacket/jacket.tuxedo.vmdl",
		"models/citizen_clothes/trousers/trousers.smart.vmdl",
		"models/citizen_clothes/shoes/SmartShoes/smartshoes.vmdl"
	};

	public override void InteractWith( LobbyPawn player )
	{
		base.InteractWith( player );
	}
}
