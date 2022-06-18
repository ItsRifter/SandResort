using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;
public partial class PHSittableProp : PHSuiteProps
{
	[Net]
	public PHPawn sittingPlayer { get; private set; }

	bool isSittingDown = false;
	public ChairCam camera;

	TimeSince timeLastSat;

	public override void Spawn()
	{
		base.Spawn();

		timeLastSat = 0;
		camera = Components.Create<ChairCam>();
		SetInteractsExclude( CollisionLayer.Player );
	}

	public override void Simulate( Client client )
	{
		base.Simulate( client );

		SimulateSitter();
	}

	void SimulateSitter()
	{
		if ( !sittingPlayer.IsValid() ) return;

		if ( IsServer && Input.Pressed( InputButton.Use ) && timeLastSat >= 1.0f )
		{
			StandUp();
			return;
		}

		sittingPlayer.SetAnimParameter( "b_grounded", true );
		sittingPlayer.SetAnimParameter( "b_sit", true );

		var aimRotation = Input.Rotation.Clamp( sittingPlayer.Rotation, 90 );

		var aimPos = sittingPlayer.EyePosition + aimRotation.Forward * 200;
		var localPos = new Transform( sittingPlayer.EyePosition, sittingPlayer.Rotation ).PointToLocal( aimPos );
		
		var duckControlFix = sittingPlayer.Controller as WalkController;

		if ( duckControlFix.Duck.IsActive && IsServer )
		{
			sittingPlayer.EyeLocalPosition += Vector3.Up * 32;
			duckControlFix.Duck.IsActive = false;
		}

		sittingPlayer.SetAnimParameter( "aim_eyes", localPos );
		sittingPlayer.SetAnimParameter( "aim_head", localPos );
		sittingPlayer.SetAnimParameter( "aim_body", localPos );
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	public void StandUp()
	{
		timeLastSat = 0;
		sittingPlayer.EnableHideInFirstPerson = true;

		sittingPlayer.LocalPosition = Vector3.Up * 10 + LocalRotation.Forward * 35;
		sittingPlayer.Parent = null;
		sittingPlayer.PhysicsBody.Enabled = true;

		sittingPlayer.Client.Pawn = sittingPlayer;
		sittingPlayer.SitProp = null;
		sittingPlayer = null;
		isSittingDown = false;

	}

	public override void Interact( PHPawn player )
	{
		base.Interact( player );

		if ( timeLastSat <= 1.0f )
			return;

		timeLastSat = 0;

		if ( !isSittingDown )
		{
			camera.SetSitter( player );

			player.SitProp = this;

			player.Parent = this;
			player.LocalPosition = Vector3.Up * 5;
			player.LocalRotation = Rotation.Identity;
			player.LocalScale = 1;
			player.PhysicsBody.Enabled = false;

			sittingPlayer = player;

			player.Client.Pawn = this;

			isSittingDown = true;
		}
	}
}

