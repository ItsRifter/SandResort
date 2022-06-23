using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;
using SandboxEditor;

[Library( "ph_subgame_area" )]
[Title( "Subgame Area" ), Category( "Sub-Games" ), Description( "Defines the area for this specific sub-game" )]
[SupportsSolid]
[HammerEntity]

[BoundsHelper("minBound", "maxBound", true, true)]

public class SubGameEnt : BaseTrigger
{
	public List<SubGameSpawnpoint> Spawnpoints;
	public List<(string modelName, Vector3 pos, Rotation rot)> Models;
	public List<SubGameBounds> Boundaries;
	public List<BrushEntity> Brushes;

	//bool isActive = false;
	bool hasSpawned = false;

	public enum SubGameArea
	{
		Unspecified,
		Monday_Massacre
	}

	[Property]
	public SubGameArea SubGameArena { get; set; } = SubGameArea.Unspecified;

	[Property, Description("Name of this area so this will be the one to be correctly loaded in after map selection")]
	public string AreaName { get; set; } = "";

	public override void Spawn()
	{
		base.Spawn();

		Spawnpoints = new List<SubGameSpawnpoint>();
		Models = new List<(string modelName, Vector3 pos, Rotation rot)>();
		Brushes = new List<BrushEntity>();
		Boundaries = new List<SubGameBounds>();
	}

	[Event.Tick.Server]
	public void Test()
	{
		if ( hasSpawned )
			return;

		foreach ( var entity in FindInBox( WorldSpaceBounds ) )
		{
			if ( entity is SubGameSpawnpoint spawn )
			{
				Spawnpoints.Add( spawn );
				continue;
			}

			if ( entity is Prop model )
				Models.Add( (model.GetModelName(), model.Position, model.Rotation) );
			

			if ( entity is BrushEntity brush )
			{
				Brushes.Add( brush );
				brush.Enabled = false;
				continue;
			}

			if(entity is SubGameBounds bounds)
			{
				Boundaries.Add( bounds );
				bounds.EnableDrawing = false;
				bounds.PhysicsClear();
				continue;
			}

			entity.Delete();
		}

		hasSpawned = true;
	}

	[ConCmd.Server("ph_subgame_test")]
	public static void LoadAreaTest(int testConduct)
	{
		switch(testConduct)
		{
			case 1:
				Event.Run( "test" );
				break;
			case 2:
				Event.Run( "test2" );
				break;
			case 3:
				Event.Run( "test3" );
				break;
		}		
	}


	[Event("test")]
	public void LoadArea()
	{
		foreach ( var bound in Boundaries )
		{
			bound.EnableDrawing = true;
			bound.SetupPhysicsFromModel( PhysicsMotionType.Static );
		}

		foreach ( var brush in Brushes )
		{
			brush.Enabled = true;
		}

		foreach ( var spawn in Spawnpoints )
		{
			spawn.IsEnabled = true;
		}

		foreach ( var model in Models )
		{
			ModelEntity newModel = new ModelEntity();
			newModel.SetModel( model.modelName );
			newModel.Position = model.pos;
			newModel.Rotation = model.rot;

			newModel.Spawn();

			newModel.SetupPhysicsFromModel( PhysicsMotionType.Static );
		}
	}

	[Event( "test2" )]
	public void RestartArea()
	{
		foreach ( var oldProp in FindInBox( WorldSpaceBounds ) )
		{
			if ( oldProp is BasePawn || oldProp is BrushEntity || oldProp is SubGameBounds || oldProp is SubGameSpawnpoint)
				continue;

			if( oldProp is ModelEntity )
			{
				oldProp.Delete();
			}
		}

		foreach ( var model in Models )
		{
			var replacement = new ModelEntity();

			replacement.SetModel( model.modelName );
			replacement.Position = model.pos;
			replacement.Rotation = model.rot;

			replacement.Spawn();

			replacement.SetupPhysicsFromModel( PhysicsMotionType.Static );
		}
	}

	[Event( "test3" )]
	public void WipeAreaAndRemove()
	{
		foreach ( var brush in Brushes )
		{
			brush.Enabled = false;
		}

		foreach ( var ent in FindInBox(WorldSpaceBounds) )
		{
			if ( ent is BasePawn || ent is BrushEntity || ent is SubGameBounds || ent is SubGameSpawnpoint )
				continue;

			if (ent is ModelEntity model )
				model.Delete();
		}

		foreach ( var spawn in Spawnpoints )
			spawn.IsEnabled = false;

		foreach ( var bound in Boundaries )
		{
			bound.EnableDrawing = false;
			bound.PhysicsClear();
		}
	}
}

