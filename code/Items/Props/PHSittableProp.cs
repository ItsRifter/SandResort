using Sandbox;

public partial class PHSittableProp : PHSuiteProps
{
	//Probably should do this with bone transform but couldn't figure it out
	public virtual Vector3 SitLocalPos => new Vector3( 0, 0, 0 );

	//In case its not a single seated prop
	public virtual bool MultiSeated => false;

	public virtual Vector3[] SitMultiLocalPos => new Vector3[] 
	{ 
		//Here would be the different local positions
	};

	[Net]
	public LobbyPawn SittingPlayer { get; private set; }

	public ChairCam Camera;
	public bool CanDirectlyInteract = true;

	TimeSince timeLastSat;
	bool isSittingDown;

	public override void Spawn()
	{
		base.Spawn();

		timeLastSat = 0;
		Camera = Components.Create<ChairCam>();
		SetInteractsExclude( CollisionLayer.Player );
	}

	public override void Simulate( Client client )
	{
		base.Simulate( client );

		SimulateSitter();
	}

	void SimulateSitter()
	{
		if ( !SittingPlayer.IsValid() ) return;

		if ( IsServer && Input.Pressed( InputButton.Use ) && timeLastSat >= 1.0f && CanDirectlyInteract )
		{
			StandUp();
			return;
		}

		SittingPlayer.SetAnimParameter( "b_grounded", true );
		SittingPlayer.SetAnimParameter( "sit", 1 );

		var aimRotation = Input.Rotation.Clamp( SittingPlayer.Rotation, 75 );

		var aimPos = SittingPlayer.EyePosition + aimRotation.Forward * 200;
		var localPos = new Transform( SittingPlayer.EyePosition, SittingPlayer.Rotation ).PointToLocal( aimPos );
		
		var duckControlFix = SittingPlayer.Controller as WalkController;

		if ( duckControlFix.Duck.IsActive && IsServer )
		{
			SittingPlayer.EyeLocalPosition += Vector3.Up * 32;
			duckControlFix.Duck.IsActive = false;
		}

		SittingPlayer.SetAnimParameter( "aim_eyes", localPos );
		SittingPlayer.SetAnimParameter( "aim_head", localPos );
		SittingPlayer.SetAnimParameter( "aim_body", localPos );
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	public void StandUp()
	{
		timeLastSat = 0;
		SittingPlayer.EnableHideInFirstPerson = true;

		SittingPlayer.SetAnimParameter( "sit", 0 );

		SittingPlayer.LocalPosition = Vector3.Up * 10 + LocalRotation.Forward * 35;
		SittingPlayer.Parent = null;
		SittingPlayer.PhysicsBody.Enabled = true;

		SittingPlayer.Client.Pawn = SittingPlayer;
		SittingPlayer.SitProp = null;
		SittingPlayer = null;
		isSittingDown = false;

	}

	public void SitDown( LobbyPawn player )
	{
		Camera.SetSitter( player );

		player.SitProp = this;

		player.Parent = this;
		player.LocalPosition = SitLocalPos;
		player.LocalRotation = Rotation.Identity;
		player.LocalScale = 1;
		player.PhysicsBody.Enabled = false;

		SittingPlayer = player;

		player.Client.Pawn = this;

		isSittingDown = true;
	}

	public override void Interact( LobbyPawn player )
	{
		base.Interact( player );

		if ( !CanDirectlyInteract )
			return;

		if ( timeLastSat <= 1.0f )
			return;

		timeLastSat = 0;

		if ( !isSittingDown )
			SitDown( player );
	}
}

