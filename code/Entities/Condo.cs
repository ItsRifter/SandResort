using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;
using SandboxEditor;

[Library("sc_condo")]
[Title("Condo"), Description("Defines the condo entity for spawn/removal"), Category("Condo")]
[BoundsHelper( "MinBoundTrigger", "MaxBoundTrigger", true, true )]
[HammerEntity]
public partial class Condo : Entity
{

	//Bounds to find props within trigger
	[Property("minBounds")]
	public Vector3 MinBoundTrigger { get; set; }

	[Property( "maxBounds" )]
	public Vector3 MaxBoundTrigger { get; set; }

	//The target condo
	[Property("targetcondo"), Title("Target Condo")]
	public EntityTarget TargetCondo { get; set; }

	[Property( "targetcondotele" ), Title( "Target Condo Teleporter" )]
	public EntityTarget TargetTeleporter { get; set; }

	[Property( "targetdest" ), Title( "Target Teleport Destination" )]
	public EntityTarget TargetDestination { get; set; }

	//MIGHT NOT BE NEEDED
	//Outputs
	protected Output OnCreation { get; set; }
	protected Output OnDestruction { get; set; }

	Condo condo;
	CondoTeleporter teleTrigger;
	Entity teleDest;

	public override void Spawn()
	{
		condo = TargetCondo.GetTargets( null ).FirstOrDefault() as Condo;
		teleTrigger = TargetTeleporter.GetTargets( null ).FirstOrDefault() as CondoTeleporter;
		teleDest = TargetDestination.GetTargets( null ).FirstOrDefault();

		condo.EnableDrawing = false;
	}

	public void CreateCondo()
	{
		OnCreation.Fire( this );

		teleTrigger.CondoTarget = teleDest;
		teleTrigger.Enable();

		condo.EnableDrawing = true;
	}

	public void DestroyCondo()
	{
		OnDestruction.Fire( this );

		condo.EnableDrawing = false;
		teleTrigger.CondoTarget = null;
		teleTrigger.Disable();
	}
}

