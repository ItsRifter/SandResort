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

[BoundsHelper("minBound", "maxBound", true, true)]

public class SubGameEnt : BaseTrigger
{
	public List<SubGameSpawnpoint> Spawnpoints;
	public List<ModelEntity> Models;
	public List<BrushEntity> BaseArea;

	bool isActive = false;
	bool hasSpawned = false;

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
		BaseArea = new List<BrushEntity>();
	}

	[Event.Tick.Server]
	public void Test()
	{
		
		if ( hasSpawned )
			return;

		foreach ( var entity in FindInBox( WorldSpaceBounds ) )
		{
			Log.Info(entity);

			if ( entity is SubGameSpawnpoint spawn )
				Spawnpoints.Add( spawn );

			if ( entity is ModelEntity model && entity is not BrushEntity )
				Models.Add( model );

			if ( entity is BrushEntity brush )
			{
				BaseArea.Add( brush );
				brush.Enabled = false;
				continue;
			}

			entity.Delete();
		}

		hasSpawned = true;
	}

	public void LoadArea()
	{
		foreach ( var spawn in Spawnpoints )
		{
			SubGameSpawnpoint newSpawn = new SubGameSpawnpoint();
			newSpawn.Position = spawn.Position;
			newSpawn.Rotation = spawn.Rotation;
		}

		foreach ( var model in Models )
		{
			ModelEntity newModel = new ModelEntity();
			newModel.Position = model.Position;
			newModel.Rotation = model.Rotation;
			newModel.Model = model.Model;

			Log.Info( model );
		}

		foreach ( var brush in BaseArea )
		{

		}
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
		foreach ( var spawn in Spawnpoints )
			spawn.Delete();

		foreach ( var brush in BaseArea )
			brush.Enabled = false;
	}
}

