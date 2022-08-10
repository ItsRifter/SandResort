using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;

public partial class PropBase : AnimatedEntity, IUse
{
	public virtual string ModelPath => "";
	public virtual string IconImage => "";
	public virtual float TimeWaitNextUse => 0.3f;

	TimeSince timeLastUse;

	public override void Spawn()
	{
		base.Spawn();

		SetModel( ModelPath );
		SetupPhysicsFromModel( PhysicsMotionType.Static );

		timeLastUse = 0;
	}

	public bool OnUse( Entity user )
	{
		if ( !IsUsable(user) )
			return false;

		InteractProp(user);

		return false;
	}

	public bool IsUsable( Entity user )
	{
		if ( timeLastUse < TimeWaitNextUse )
			return false;

		return true;
	}

	public virtual void InteractProp( Entity user )
	{
		//Do stuff
		timeLastUse = 0;
	}
}
