using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;

public partial class BasePawn
{
	PropBase curPreviewProp;
	float rotAngle;


	public void CreatePreviewProp(PropBase newProp)
	{
		if( curPreviewProp != null )
		{
			curPreviewProp.Delete();
			curPreviewProp = null;
		}

		rotAngle = 0;

		curPreviewProp = newProp;
		curPreviewProp.IsPreview = true;

		curPreviewProp.RenderColor = new Color( 255, 255, 255, 0.65f );
	}

	public bool CanPlaceItem(Vector3 surfaceNormal)
	{
		if ( !surfaceNormal.IsNearlyZero() )
			return true;

		return false;
	}

	public void SimulatePlacing()
	{
		if ( curPreviewProp == null )
			return;

		var tr = Trace.Ray( EyePosition, EyePosition + EyeRotation.Forward * 95 )
			.Ignore( curPreviewProp )
			.Ignore( this )
			.Run();

		if ( rotAngle > 360.0f )
			rotAngle = 1;
		else if ( rotAngle < 0 )
			rotAngle = 359.0f;

		rotAngle += Input.MouseWheel * 5;

		curPreviewProp.Rotation = Rotation.FromYaw( rotAngle );
		curPreviewProp.Position = tr.EndPosition;

		if ( CanPlaceItem( tr.Normal ) )
			curPreviewProp.RenderColor = new Color(0, 255, 0, 0.65f);
		else
			curPreviewProp.RenderColor = new Color( 255, 0, 0, 0.65f );

		if ( IsServer )
		{
			if(Input.Pressed(InputButton.PrimaryAttack) && CanPlaceItem( tr.Normal ) )
			{
				var prop = TypeLibrary.Create<PropBase>(curPreviewProp.GetType().FullName);
				prop.Position = curPreviewProp.Position;
				prop.Rotation = curPreviewProp.Rotation;
				prop.Spawn();

				curPreviewProp.Delete();
				curPreviewProp = null;
			}
		}
	}
}

