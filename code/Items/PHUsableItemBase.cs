using Sandbox;

public class PHUsableItemBase : BaseCarriable
{
	public virtual string ItemName => "Usable Base Item";
	public virtual string ItemDesc => "A base item for other usable items to derive from";

	public virtual Model ItemModel => Model.Load( "" );

	public virtual int ItemCost => 1;

	public override void Spawn()
	{
		base.Spawn();

		Model = ItemModel;

		MoveType = MoveType.None;
		CollisionGroup = CollisionGroup.Interactive;
		PhysicsEnabled = false;
		UsePhysicsCollision = false;
		EnableHideInFirstPerson = true;
		EnableShadowInFirstPerson = true;
	}

	public override void ActiveStart( Entity ent )
	{
		base.ActiveStart( ent );
		Log.Info( "START" );
	}

	public virtual void PickUpItem()
	{
		//Do pickup sounds
		//TODO: make an inventory stuff for adding
	}

	public override void Simulate( Client cl )
	{
		base.Simulate( cl );
	}

	public bool IsUsable( Entity user )
	{
		return true;
	}

	public bool OnUse( Entity user )
	{
		return false;
	}
}
