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

		if(Input.Pressed(InputButton.PrimaryAttack))
		{
			if ( tr.Normal.z != 1 ) return;

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

