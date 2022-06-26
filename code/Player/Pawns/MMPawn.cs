using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;

public partial class MMPawn : BasePawn
{

	public PHInventorySystem MMInventory;

	TimeSince timeDied;

	public MMPawn()
	{
		MMInventory = new PHInventorySystem( this );
	}

	void FindSpawnpoint()
	{
		if ( IsClient )
			return;

		var mondaySpawns = All.OfType<SubGameSpawnpoint>()
			.OrderBy( x => Guid.NewGuid() )
			.FirstOrDefault();

		if ( mondaySpawns.SubGameType == SubGameSpawnpoint.SubGameSpawn.Monday_Massacre && mondaySpawns.IsEnabled )
		{
			Transform = mondaySpawns.Transform;
		}
	}

	public override void Spawn()
	{
		SetUpPlayer();

		FindSpawnpoint();
		
		EnableLagCompensation = true;

		CreateHull();
		ResetInterpolation();
	}

	public override void Simulate( Client cl )
	{
		base.Simulate( cl );

		if(LifeState == LifeState.Dead && timeDied >= 3.0f)
		{
			Log.Info( "Respawning" );
			Respawn();
		}
	}

	public override void Respawn()
	{
		SetUpPlayer();

		MMInventory.DeleteContents();

		MMInventory.Add(new PHBaseWep(), true);

		LifeState = LifeState.Alive;

		//Deletes the corpse if valid
		DestroyCorpse( To.Everyone );

		FindSpawnpoint();
	}

	public override void TakeDamage( DamageInfo info )
	{
		base.TakeDamage( info );
	}

	public override void OnKilled()
	{
		base.OnKilled();

		timeDied = 0;
	}
}

