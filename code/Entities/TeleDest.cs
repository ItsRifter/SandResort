using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;
using SandboxEditor;

[Library( "sc_teledest" )]
[HammerEntity, EditorModel( "models/editor/playerstart.vmdl", FixedBounds = true )]
[Title( "Teleport Destination" ), Category( "Triggers" ), Icon( "place" )]
public class TeleDest : Entity
{

}

