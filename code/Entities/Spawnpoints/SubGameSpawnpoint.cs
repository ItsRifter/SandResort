using Sandbox;
using SandboxEditor;

[Library("ph_subgame_spawnpoint")]
[Title("Subgame Spawnpoint"), Category( "Sub-Games" ), Description("A unique spawnpoint for subgames")]
[EditorModel( "models/dev/playerstart_tint.vmdl" )]
[RenderFields]
[HammerEntity]
public class SubGameSpawnpoint : ModelEntity
{
	//If its enabled, allow players in the sub-game to spawn there
	public bool IsEnabled;

	public enum SubGameSpawn
	{
		Unspecified,
		Monday_Massacre
	}

	[Property]
	public SubGameSpawn SubGameType { get; set; } = SubGameSpawn.Unspecified;

	public override void Spawn()
	{
		SetModel( "models/dev/playerstart_tint.vmdl" );

		IsEnabled = false;
		EnableDrawing = false;
		SetInteractsExclude(CollisionLayer.Player | CollisionLayer.Solid | CollisionLayer.Debris);
	}
}

