using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;
using SandboxEditor;

[Library( "ph_suite_reception_spawn" )]
[Title( "Suite Receptionist" ), Description( "Indicates where the suite receptionist should spawn at" )]
[EditorModel( "models/citizen/citizen.vmdl" )]
[HammerEntity]
public partial class SuiteReception : Entity
{
	public override void Spawn()
	{
		base.Spawn();

		var receptionist = TypeLibrary.Create<SuiteReceptionist>( "SuiteReceptionist" );
		receptionist.Position = Position;
		receptionist.Rotation = Rotation;
		receptionist.Spawn();
	}
}
