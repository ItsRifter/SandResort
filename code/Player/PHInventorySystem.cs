using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;

public partial class PHInventorySystem : IBaseInventory
{
	public Entity Owner { get; init; }

	public List<Entity> InventoryList = new List<Entity>();

	public IList<(string, string)> ClientInventory { get; set; } = new List<(string, string)>();

	int maxHoldingAmount = 20;

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

	public PHInventorySystem( Entity owner )
	{
		Owner = owner;
	}


	public void EquipCosmetic(Entity ent)
	{
		Host.AssertServer();

		ent.Parent = Owner;

		(Owner as PHPawn).ActiveChildren.Add( ent );
		ent.SetParent( Owner, true );
	}

	public void UseItem(Entity ent)
	{
		Host.AssertServer();

		InventoryList.Remove( ent );
	}

	public bool AddItem(Entity ent)
	{
		Host.AssertServer();

		if ( ent.Owner != null )
			return false;

		if ( !CanAdd( ent ) )
			return false;

		InventoryList.Add( ent );

		if( ent is PHSuiteProps suiteItem )
			(Owner as PHPawn).UpdateClientInventory(ent.ClassName, suiteItem.SuiteItemImage);
		else if (ent is PHUsableItemBase usableItem)
			(Owner as PHPawn).UpdateClientInventory( ent.ClassName, "" );

		return true;
	}

	public List<string> GetAllItemsString()
	{
		List<string> allItemsString = new List<string>();

		foreach ( var item in InventoryList )
		{
			allItemsString.Add( item.ClassName );
		}

		return allItemsString;
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
		if( InventoryList.Count < maxHoldingAmount )
			return true;

		if ( ent is BaseCarriable bc && bc.CanCarry( Owner ) )
			return true;

		return false;
	}

	public bool Contains( Entity ent )
	{
		return false;
	}

	public int Count() => InventoryList.Count;

	public void DeleteContents()
	{
		Host.AssertServer();

		foreach ( var item in InventoryList.ToArray() )
		{
			item.Delete();
		}

		InventoryList.Clear();
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

