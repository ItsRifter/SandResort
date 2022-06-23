using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;

public partial class FurnitureShop : ShopKeeperBase
{
	public override string NPCName => "Furniture Haven";
	public override List<string> ClothingModels => new List<string>
	{
		"models/citizen_clothes/hat/hat_hardhat.vmdl",

	};
	public override void InteractWith( LobbyPawn player )
	{
		base.InteractWith( player );
	}
}

