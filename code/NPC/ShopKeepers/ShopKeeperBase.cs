﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;

public partial class ShopKeeperBase : AnimatedEntity
{
	public virtual string NPCName => "Base Shop NPC";
	public virtual string ModelPath => "models/citizen/citizen.vmdl";

	public override void Spawn()
	{
		base.Spawn();

		SetModel( ModelPath );

		EyePosition = Position + Vector3.Up * 64;
		CollisionGroup = CollisionGroup.Player;
		SetupPhysicsFromCapsule( PhysicsMotionType.Keyframed, Capsule.FromHeightAndRadius( 72, 8 ) );

		EnableHitboxes = true;
		EnableLagCompensation = true;

		SetBodyGroup( 1, 0 );
	}


	public virtual void InteractWith(PHPawn player)
	{
		if ( player.OpenShop )
		{
			player.OpenShop = false;
			player.ShopKeeper = null;
		}
		else
		{
			player.OpenShop = true;
			player.ShopKeeper = this;
		}
	}

	public override void TakeDamage( DamageInfo info )
	{
		return;
	}
	public override void OnKilled()
	{
		base.OnKilled();
	}
}
