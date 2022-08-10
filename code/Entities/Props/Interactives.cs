using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;

public class ClassicRadio : PropBase, IUse
{
	public override string ModelPath => "models/radio/oldradio/oldradio.vmdl";
	public override string IconImage => "ui/icon_classicradio.png";

	public override float TimeWaitNextUse => 1.5f;

	Sound sound;

	public override void InteractProp( Entity user )
	{

		if (!sound.Finished)
		{
			using ( Prediction.Off() )
				sound.Stop();

			base.InteractProp( user );
			return;
		}

		sound = Sound.FromEntity( "classic_radio", this );

		base.InteractProp( user );
	}
}

public class Giftbox : PropBase, IUse
{
	public override string ModelPath => "models/special/gift/gift.vmdl";
	public override string IconImage => "ui/icon_gift.png";

	public override float TimeWaitNextUse => 0.5f;

	public override void InteractProp( Entity user )
	{
		Particles.Create( "particles/confetti/confetti_splash.vpcf", this, false );

		Sound.FromEntity( "gift_reveal", this );

		Delete();
	}
}

