﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;

public partial class AdminNPC : PHBaseNPC
{
	public override void InteractWith( PHPawn player )
	{
		base.InteractWith( player );

		if ( !PHGame.AdminList.Contains( player.Client.Name ) )
			return;
	}
}

