using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;

public class RagdollCamera : CameraMode
{
	Vector3 FocusPoint;

	bool firstPerson;
	bool thirdPerson;

	public RagdollCamera()
	{

	}

	public RagdollCamera(bool enableThirdPerson = true, bool enableFirstPerson = false) : base()
	{
		thirdPerson = enableThirdPerson;
		firstPerson = enableFirstPerson;
	}

	public override void Activated()
	{
		base.Activated();

		FocusPoint = CurrentView.Position - GetViewOffset();
		FieldOfView = CurrentView.FieldOfView;
	}

	public override void Update()
	{
		var client = Local.Client;
		if ( client == null ) return;

		// lerp the focus point
		FocusPoint = Vector3.Lerp( FocusPoint, GetSpectatePoint(), Time.Delta * 5.0f );

		Position = FocusPoint + GetViewOffset();
		Rotation = Input.Rotation;
		FieldOfView = FieldOfView.LerpTo( 50, Time.Delta * 3.0f );

		Viewer = null;
	}

	public virtual Vector3 GetSpectatePoint()
	{
		if ( Local.Pawn is LobbyPawn player && player.Corpse.IsValid() )
		{
			return player.Corpse.PhysicsGroup.MassCenter;
		}

		return Local.Pawn.Position;
	}

	public virtual Vector3 GetViewOffset()
	{
		var player = Local.Client;
		if ( player == null ) return Vector3.Zero;

		return Input.Rotation.Forward * (-130 * 1) + Vector3.Up * (20 * 1);
	}
}
