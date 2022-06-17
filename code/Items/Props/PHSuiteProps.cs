﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;
public partial class PHSuiteProps : ModelEntity, IUse
{
	public virtual string SuiteItemName => "Suite Base Item";
	public virtual string SuiteItemDesc => "A base item for other suite items to derive from";
	public virtual string SuiteItemImage => "ui/ph_icon_missing.png";
	public virtual int SuiteItemCost => 1;
	public virtual Model WorldModel => null;

	[Net]
	public bool IsPreview { get; set; } = false;

	public bool IsMovingFrom { get; set; } = false;

	public enum ShopType
	{
		Unspecified,
		Bar,
		Furniture,
		Electric,
		Music,
		Wanderer,
	}

	public virtual ShopType ShopSeller => ShopType.Unspecified;

	public override void Spawn()
	{
		Model = WorldModel;

		if(IsPreview)
		{
			SetupPhysicsFromModel( PhysicsMotionType.Static );
			EnableAllCollisions = false;
		}
		else
			SetupPhysicsFromModel( PhysicsMotionType.Keyframed );
	}

	protected override void OnPhysicsCollision( CollisionEventData eventData )
	{
		base.OnPhysicsCollision( eventData );
	}

	public override void Simulate( Client client )
	{
		if ( IsPreview )
			return;
	}

	public virtual void Interact( PHPawn player )
	{
		//Do interactive stuff
	}

	public bool OnUse( Entity user )
	{
		if ( !IsUsable( user ) )
			return false;

		if ( user is PHPawn player )
		{
			Interact( player );
			return true;
		}

		return false;
	}

	public bool IsUsable( Entity user )
	{
		return !IsPreview;
	}
}
