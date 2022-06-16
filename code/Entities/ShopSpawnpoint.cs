using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;
using SandboxEditor;

[Library( "ph_shop_spawn" )]
[Title( "Shopkeeper" ), Description( "Indicates where shopkeepers should spawn at" )]
[EditorModel( "models/citizen/citizen.vmdl" )]
[HammerEntity]
public partial class ShopSpawnpoint : Entity
{
	public enum ShopType
	{
		Unspecified,
		Bar,
		Furniture,
		Electric
	}

	[Property]
	public ShopType ShopToSpawn { get; set; } = ShopType.Unspecified;

	public override void Spawn()
	{
		base.Spawn();

		string spawnShop = "";

		switch(ShopToSpawn)
		{
			case ShopType.Bar:
				spawnShop = "BarShop";
				break;

			case ShopType.Furniture:
				spawnShop = "FurnitureShop";
				break;

			case ShopType.Electric:
				spawnShop = "ElectricShop";
				break;
		}

		if ( string.IsNullOrEmpty( spawnShop ) )
			return;

		var shop = TypeLibrary.Create<ShopKeeperBase>( spawnShop );
		shop.Position = Position;
		shop.Rotation = Rotation;
		shop.Spawn();
	}
}
