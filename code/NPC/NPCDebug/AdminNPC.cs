using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;

public partial class AdminNPC : PCBaseNPC
{
	public override void InteractWith( PCPawn player )
	{
		base.InteractWith( player );

		if ( !PCGame.AdminList.Contains( player.Client.Name ) )
			return;
	}
}

