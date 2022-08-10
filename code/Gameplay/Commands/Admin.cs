using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;

public partial class SCGame
{
	[ConCmd.Admin("sc_ent_spawn")]
	public static void EntitySpawnCMD(string entName)
	{
		var player = ConsoleSystem.Caller.Pawn as BasePawn;

		if ( player == null )
			return;

		Entity ent = null;
		Log.Info( TypeLibrary.GetTypeByName( entName ) );
		switch(TypeLibrary.GetTypeByName(entName).ToString())
		{
			case "ShopKeeperBase":
				ent = TypeLibrary.Create<ShopKeeperBase>( entName );
				break;
		}

		if ( ent == null )
		{
			Log.Error( "Invalid entity, check the name that it exists" );
			return;
		}

		var tr = Trace.Ray( player.EyePosition, player.EyePosition + player.EyeRotation.Forward * 150 )
			.Ignore( player )
			.Run();

		ent.Position = tr.EndPosition;
	}
}
