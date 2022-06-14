using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;
public partial class PHSuiteProps : Prop, IUse
{
	public virtual int SuiteItemCost => 1;
	public virtual Model WorldModel => null;

	public bool IsPreview = false;

	public override void Spawn()
	{
		base.Spawn();

		Model = WorldModel;

		if(IsPreview)
		{
			SetupPhysicsFromModel( PhysicsMotionType.Static );
			EnableAllCollisions = false;
		}
		else
		{
			SetupPhysicsFromModel( PhysicsMotionType.Keyframed );
			EnableAllCollisions = true;
		}

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
