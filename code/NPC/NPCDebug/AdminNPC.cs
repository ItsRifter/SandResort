﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;

public partial class AdminNPC : ShopKeeperBase
{
	public override string NPCName => "Admin Test Shop";

	public override void InteractWith( PHPawn player )
	{
		base.InteractWith( player );
	}
}

