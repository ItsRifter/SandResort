using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;

public partial class PHPawn
{
	public PHSuiteProps previewProp;

	public void SimulatePropPlacement()
	{
		if ( previewProp == null ) return;

		var tr = Trace.Ray( EyePosition, EyePosition + EyeRotation.Forward * 150 )
			.Ignore( this )
			.Ignore( previewProp )
			.Radius( 2 )
			.Run();

		previewProp.Position = tr.EndPosition;

		if ( tr.Normal.z != 1 || FindInBox( previewProp.WorldSpaceBounds ).Count() > 0 )
			previewProp.RenderColor = new Color( 165, 0, 0, 0.5f);
		else
			previewProp.RenderColor = new Color( 0, 255, 0, 0.5f );

		Log.Info( tr.Normal );

		if (Input.Pressed(InputButton.PrimaryAttack))
		{
			if ( tr.Normal.z != 1 || FindInBox( previewProp.WorldSpaceBounds ).Count() > 0 ) return;

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

