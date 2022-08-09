using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;

public partial class PHGame
{
	[ConVar.ClientData( "cl_showfps", Saved = true )]
	public bool ShowFpsEnabled { get; set; }
}

