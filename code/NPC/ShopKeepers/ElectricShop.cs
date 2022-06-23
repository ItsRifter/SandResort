using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;

public partial class ElectricShop : ShopKeeperBase
{
	public override string NPCName => "Electrical Shop";

	public override List<string> ClothingModels => new List<string>
	{
		"models/citizen_clothes/jacket/LongSleeve/Models/longsleeve.vmdl",
		"models/citizen_clothes/jacket/LongSleeve/Models/jeans.vmdl",
		"models/citizen_clothes/shoes/shoes.workboots.vmdl"
	};

	public override void InteractWith( LobbyPawn player )
	{
		base.InteractWith( player );
	}
}
