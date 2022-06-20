using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;
using SandboxEditor;

[Library( "ph_subgame_area" )]
[Title( "Subgame Area" ), Description( "Defines the area for this specific sub-game" )]
[SupportsSolid]
[HammerEntity]

public class SubGameEnt : ModelEntity
{
	public List<SubGameSpawnpoint> Spawnpoints;
	public List<ModelEntity> Models;

	public enum SubGameArea
	{
		Unspecified,
		Monday_Massacre
	}

	[Property]
	public SubGameArea SubGameArena { get; set; } = SubGameArea.Unspecified;

	[Property, Description("Name of this area so this will be the one to be correctly loaded in")]
	public string AreaName { get; set; } = "";

	public override void Spawn()
	{
		base.Spawn();

		Spawnpoints = new List<SubGameSpawnpoint>();
		Models = new List<ModelEntity>();

		foreach ( var entity in FindInBox(WorldSpaceBounds) )
		{
			if ( entity is SubGameSpawnpoint spawn )
				Spawnpoints.Add( spawn );

			if ( entity is ModelEntity model )
				Models.Add( model );

			entity.Delete();
		}

		RenderColor = new Color( 255, 255, 255, 0 );
		EnableAllCollisions = false;
	}

	public void LoadArea()
	{
		foreach ( var spawn in Spawnpoints )
		{
			SubGameSpawnpoint newSpawn = new SubGameSpawnpoint();
			newSpawn.Position = spawn.Position;
			newSpawn.Rotation = spawn.Rotation;
		}

		RenderColor = new Color( 255, 255, 255, 1 );
		EnableAllCollisions = true;
	}

	public void RestartArea()
	{
		foreach ( var model in Models )
		{
			var replacement = new ModelEntity();

			replacement.Position = model.Position;
			replacement.Rotation = model.Rotation;
			
			replacement.Spawn();

			model.Delete();
		}
	}

	public void WipeAreaAndRemove()
	{
		RenderColor = new Color( 255, 255, 255, 0 );
		EnableAllCollisions = false;
	}
}

