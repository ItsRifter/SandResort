using Sandbox;

public partial class PHGame
{
	[ConVar.ClientDataAttribute( "cl_showfps", Saved = true )]
	public bool ShowFpsEnabled { get; set; }
}

