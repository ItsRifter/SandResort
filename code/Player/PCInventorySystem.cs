using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;

public partial class PCInventorySystem : IBaseInventory
{
	public Entity Owner { get; init; }
	public List<Entity> HoldingList = new List<Entity>();

	public virtual Entity Active
	{
		get
		{
			return (Owner as Player)?.ActiveChild;
		}

		set
		{
			if ( Owner is Player player )
			{
				player.ActiveChild = value;
			}
		}
	}

	public PCInventorySystem( Entity owner )
	{
		Owner = owner;
	}

	public bool Add( Entity ent, bool makeactive = false )
	{
		Host.AssertServer();

		if ( ent.Owner != null )
			return false;

		if ( !CanAdd( ent ) )
			return false;

		if ( ent is not BaseCarriable carriable )
			return false;

		if ( !carriable.CanCarry( Owner ) )
			return false;

		ent.Parent = Owner;

		carriable.OnCarryStart( Owner );

		return true;
	}

	public virtual bool CanAdd( Entity ent )
	{
		if ( ent is BaseCarriable bc && bc.CanCarry( Owner ) )
			return true;

		return false;
	}

	public bool Contains( Entity ent )
	{
		return false;
	}

	public int Count() => HoldingList.Count;

	public void DeleteContents()
	{
		Host.AssertServer();

		foreach ( var item in HoldingList.ToArray() )
		{
			item.Delete();
		}

		HoldingList.Clear();
	}

	public bool Drop( Entity ent )
	{
		return false;
	}

	public Entity DropActive()
	{
		return null;
	}

	public int GetActiveSlot()
	{
		return -1;
	}

	public Entity GetSlot( int i )
	{
		return null;
	}

	public void OnChildAdded( Entity child )
	{
		
	}

	public void OnChildRemoved( Entity child )
	{

	}

	public bool SetActive( Entity ent )
	{
		return false;
	}

	public bool SetActiveSlot( int i, bool allowempty )
	{
		return false;
	}

	public bool SwitchActiveSlot( int idelta, bool loop )
	{
		return false;
	}
}

