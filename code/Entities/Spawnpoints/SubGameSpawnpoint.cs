using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;
using SandboxEditor;

[Library("ph_subgame_spawnpoint")]
[Title("Subgame Spawnpoint"), Description("A unique spawnpoint for subgames")]
[EditorModel( "models/dev/playerstart_tint.vmdl" )]
[RenderFields]
[HammerEntity]
public class SubGameSpawnpoint : Entity
{
	public enum SubGameSpawn
	{
		Unspecified,
		Monday_Massacre
	}

	[Property]
	public SubGameSpawn SubSpawn { get; set; } = SubGameSpawn.Unspecified;
}

