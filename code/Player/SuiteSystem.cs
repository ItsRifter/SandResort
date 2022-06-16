using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;

public interface ISuiteProp
{
	Model Model { get; }
	Vector3 Pos { get; }
	Rotation Rot { get; }

}
public class SuitePropInfo : ISuiteProp
{
	public Model Model { get; set; }
	public Vector3 Pos { get; set; }
	public Rotation Rot { get; set; }
}

public partial class PHPawn
{
	public PHSuiteProps previewProp;

	public SuiteRoomEnt CurSuite;

	float scrollRot = 0;

	public bool CheckPlacementSurface(Vector3 surface)
	{
		if ( surface.x == 1 || surface.x == -1)
			return true;

		if ( surface.y == 1 || surface.y == -1 )
			return true;

		if ( surface.z == 1 || surface.z == -1 )
			return true;

		return false;
	}

	[ClientRpc]
	public void ShowSittingAngle(PHSuiteProps prop)
	{
		DebugOverlay.Line( prop.Position + Vector3.Up * 16, prop.Position + Vector3.Up * 16 + prop.Rotation.Forward * 35 );
	}

	public void SimulatePropPlacement()
	{
		if ( previewProp == null ) return;

		var tr = Trace.Ray( EyePosition, EyePosition + EyeRotation.Forward * 150 )
			.WithoutTags( "PH_Player" )
			.Ignore( previewProp )
			.Run();

		if( previewProp is PHSittableProp && previewProp.Owner == this )
			ShowSittingAngle(To.Single(this), previewProp);

		scrollRot += Input.MouseWheel * 5;

		previewProp.Rotation = Rotation.FromYaw( scrollRot );
		previewProp.Position = tr.EndPosition;
		
		if ( FindInBox( previewProp.WorldSpaceBounds ).Count() > 0 )
			previewProp.RenderColor = new Color( 165, 0, 0, 0.5f );
		else if ( !CheckPlacementSurface( tr.Normal ) )
			previewProp.RenderColor = new Color( 165, 0, 0, 0.5f);
		else
			previewProp.RenderColor = new Color( 0, 255, 0, 0.5f );

		if (Input.Pressed(InputButton.PrimaryAttack))
		{
			if ( FindInBox( previewProp.WorldSpaceBounds ).Count() > 0 )
				return;
			else if ( tr.Normal.z != 1 ) 
				return;

			var placedProp = TypeLibrary.Create<PHSuiteProps>( previewProp.GetType().FullName );
			placedProp.Model = previewProp.Model;
			placedProp.Position = previewProp.Position;
			placedProp.Rotation = previewProp.Rotation;
			placedProp.Spawn();
			placedProp.SetModel(previewProp.GetModelName());

			foreach ( var item in PHInventory.InventoryList.ToArray() )
			{
				if((item as PHSuiteProps).SuiteItemName == previewProp.SuiteItemName)
				{
					PHInventory.InventoryList.Remove( item );

					item.Delete();
					break;
				}
			}

			UpdateClientInventory( To.Single( this ), placedProp.ClassName, false );

			previewProp.Delete();
			previewProp = null;
		}
	}
}

