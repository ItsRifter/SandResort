using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;

public partial class PHPawn
{
	public PHSuiteProps previewProp;

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


	public void ShowSittingAngle()
	{
		DebugOverlay.Line( previewProp.Position + Vector3.Up * 16, previewProp.Position + Vector3.Up * 16 +  previewProp.Rotation.Forward * 35 );
	}

	public void SimulatePropPlacement()
	{
		if ( previewProp == null ) return;

		var tr = Trace.Ray( EyePosition, EyePosition + EyeRotation.Forward * 150 )
			.WithoutTags( "PH_Player" )
			.Ignore( previewProp )
			.Run();

		if( previewProp is PHSittableProp )
			ShowSittingAngle();

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

			previewProp.Delete();
			previewProp = null;
		}
	}
}

