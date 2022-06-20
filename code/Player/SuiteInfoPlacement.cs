using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;

public interface ISuiteProp
{
	string PropName { get; }
	Model Model { get; }
	Vector3 Pos { get; }
	Rotation Rot { get; }

}
public class SuitePropInfo : ISuiteProp
{
	public string PropName { get; set; }
	public Model Model { get; set; }
	public Vector3 Pos { get; set; }
	public Rotation Rot { get; set; }
}

public partial class PHPawn
{
	[Net]
	public PHSuiteProps PreviewProp { get; set; }

	public SuiteRoomEnt CurSuite;

	float scrollRot = 0;

	public bool CheckPlacementSurface(Vector3 surface)
	{
		if ( surface.x == 1 || surface.x == -1 )
			return true;

		if ( surface.y == 1 || surface.y == -1 )
			return true;

		if ( surface.z == 1 || surface.z == -1 )
			return true;

		return false;
	}

	public void ShowSittingAngle()
	{
		if ( IsClient )
			return;
		
		DebugOverlay.Line( PreviewProp.Position + Vector3.Up * 16, PreviewProp.Position + Vector3.Up * 16 + PreviewProp.Rotation.Forward * 35 );
		
	}

	public TimeSince timeToWaitPlacing;

	public void SimulatePropPlacement()
	{
		if ( PreviewProp == null ) return;

		if ( PreviewProp.IsMovingFrom && Input.Pressed(InputButton.SecondaryAttack) && IsServer )
		{
			PHInventory.InventoryList.Add( PreviewProp );
			UpdateClientInventory( PreviewProp.ClassName );

			All.OfType<PHSuiteProps>().FirstOrDefault( x => x.Name == PreviewProp.Name ).Delete();

			PreviewProp.Delete();
			PreviewProp = null;

			return;
		} 
		else if (!PreviewProp.IsMovingFrom && Input.Pressed( InputButton.SecondaryAttack ) && IsServer )
		{
			PreviewProp.Delete();
			PreviewProp = null;

			return;
		}

		var mouseTrace = Trace.Ray( Input.Cursor.Origin, Input.Cursor.Project( 180 ) )
			.Ignore( this )
			.Ignore( PreviewProp )
			.Run();

		if ( PreviewProp is PHSittableProp && PreviewProp.PropOwner == this )
			ShowSittingAngle();
		
		if(IsServer)
		{
			scrollRot += Input.MouseWheel * 5;

			PreviewProp.Position = mouseTrace.EndPosition;
			PreviewProp.Rotation = Rotation.FromYaw( scrollRot );

			if ( CurSuite == null )
				PreviewProp.RenderColor = new Color( 165, 0, 0, 0.5f );
			else if ( FindInBox( PreviewProp.WorldSpaceBounds ).Count() > 0 )
				PreviewProp.RenderColor = new Color( 165, 0, 0, 0.5f );
			else if ( !CheckPlacementSurface( mouseTrace.Normal ) )
				PreviewProp.RenderColor = new Color( 165, 0, 0, 0.5f );
			else
				PreviewProp.RenderColor = new Color( 0, 255, 0, 0.5f );
		}


		if (Input.Pressed(InputButton.PrimaryAttack) && IsServer)
		{
			if ( timeToWaitPlacing <= 0.5f )
				return;

			if ( FindInBox( PreviewProp.WorldSpaceBounds ).Count() > 0 )
				return;

			if ( !CheckPlacementSurface( mouseTrace.Normal ) )
				return;
			
			if ( CurSuite == null )
				return;

			bool isInSuiteArea = false;

			foreach ( var ownerSuite in FindInBox( CurSuite.WorldSpaceBounds ) )
			{
				if ( ownerSuite is PHPawn player && player == CurSuite.SuiteOwner )
					isInSuiteArea = true;
			}

			if ( !isInSuiteArea )
				return;

			if( !PreviewProp.IsMovingFrom )
			{
				var placedProp = TypeLibrary.Create<PHSuiteProps>( PreviewProp.GetType().FullName );

				placedProp.Model = PreviewProp.Model;
				placedProp.Position = PreviewProp.Position;
				placedProp.Rotation = PreviewProp.Rotation;
				placedProp.PropOwner = this;

				placedProp.Spawn();
				placedProp.SetParent( CurSuite );

				CheckOrUpdateAchievement( "Suite OCD", "OCD" );

				foreach ( var item in PHInventory.InventoryList.ToArray() )
				{
					if ( (item as PHSuiteProps).SuiteItemName == PreviewProp.SuiteItemName )
					{
						PHInventory.InventoryList.Remove( item );

						item.Delete();
						break;
					}
				}

				UpdateClientInventory( placedProp.ClassName, false );
			} 
			else if ( PreviewProp.IsMovingFrom )
			{
				var movedProp = All.OfType<PHSuiteProps>().FirstOrDefault( x => x.Name == PreviewProp.Name );
				movedProp.Position = PreviewProp.Position;
				movedProp.Rotation = PreviewProp.Rotation;

				CheckOrUpdateAchievement( "Suite OCD", "OCD" );
			}

			if ( IsServer )
			{
				PreviewProp.Delete();
				PreviewProp = null;
			}
		}
	}
}

