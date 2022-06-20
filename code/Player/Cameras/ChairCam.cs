using Sandbox;
using System;

public partial class ChairCam : CameraMode
{
	[Net]
	protected LobbyPawn sitter { get; set; }

	protected virtual float OrbitSmoothingSpeed => 25.0f;
	protected virtual float MinOrbitPitch => -65.0f;
	protected virtual float MaxOrbitPitch => 65.0f;
	protected virtual float FixedOrbitPitch => 10.0f;
	protected virtual float OrbitHeight => 90.0f;
	protected virtual float OrbitDistance => 150.0f;

	private Angles orbitAngles;
	private Rotation orbitYawRot;
	private Rotation orbitPitchRot;
	private float carPitch;

	public override void Activated()
	{
		orbitAngles = Angles.Zero;
		orbitYawRot = Rotation.Identity;
		orbitPitchRot = Rotation.Identity;
		carPitch = 0;

		orbitYawRot = Rotation.FromYaw( Entity.Rotation.Yaw() );
		orbitPitchRot = Rotation.Identity;
		orbitAngles = (orbitYawRot * orbitPitchRot).Angles();
	}

	public override void Update()
	{
		var chair = Entity as ModelEntity;
		if ( !chair.IsValid() ) return;

		var body = chair.PhysicsBody;
		if ( !body.IsValid() ) return;
	
		var slerpAmount = Time.Delta * OrbitSmoothingSpeed;

		orbitYawRot = Rotation.Slerp( orbitYawRot, Rotation.From( 0.0f, orbitAngles.yaw, 0.0f ), slerpAmount );
		orbitPitchRot = Rotation.Slerp( orbitPitchRot, Rotation.From( orbitAngles.pitch + carPitch, 0.0f, 0.0f ), slerpAmount );
		
		DoFirstPerson( chair, body );
	}

	public void SetSitter(LobbyPawn playerPawn)
	{
		sitter = playerPawn;
	}

	private void DoFirstPerson( ModelEntity chair, PhysicsBody body )
	{
		Rotation = orbitYawRot * orbitPitchRot;

		Position = sitter.EyePosition + Vector3.Down * 20 + chair.Rotation.Forward * 1.25f;

		Viewer = null;
	}

	public override void BuildInput( InputBuilder input )
	{
		base.BuildInput( input );

		var pawn = Local.Pawn;
		if ( pawn == null ) return;

		if ( (Math.Abs( input.AnalogLook.pitch ) + Math.Abs( input.AnalogLook.yaw )) > 0.0f )
		{
			orbitAngles.yaw += input.AnalogLook.yaw;
			orbitAngles.pitch += input.AnalogLook.pitch;
			orbitAngles = orbitAngles.Normal;
			orbitAngles.pitch = orbitAngles.pitch.Clamp( MinOrbitPitch, MaxOrbitPitch );
		}

		input.ViewAngles = orbitAngles;
		input.ViewAngles = input.ViewAngles.Normal;
	}
}
