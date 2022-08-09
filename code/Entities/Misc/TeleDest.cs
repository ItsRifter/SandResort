﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;
using SandboxEditor;

[Library( "sc_teleport_destination" )]
[Title("Teleport Destination"), Category("Misc"), Description("Indicates where entities (including players) should teleport at")]
[EditorModel( "models/dev/playerstart_tint.vmdl" )]
[RenderFields]
[HammerEntity]
public partial class TeleDest : Entity
{
}

