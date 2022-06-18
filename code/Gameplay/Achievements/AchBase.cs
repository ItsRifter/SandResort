using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;

public partial class AchBase 
{
	public virtual string AchName => "Base Achievement";
	public virtual string AchDesc => "The base of all achievements";

	//GMT Award sound, we should replace it with our own
	public virtual string AchUnlockSound => "ach_award";
	public virtual int AchProgressMax => 1;
	public virtual int AchCoinsReward => 1;
	public bool HasCompleted = false;

	public virtual List<Entity> AchItemRewards => new List<Entity>()
	{
		//Add class items here
		//Example: new Jetpack();
	};

	public int AchProgress = 0;

	public void ServerAutoGiveAchievement( PHPawn player, AchBase achievement )
	{
		player.PlaySound( AchUnlockSound );
		player.AchList.Add( achievement.AchName );
		ConsoleSystem.Run( "say", $"{player.Client.Name} has earned the achievement: {achievement.AchName}", true );
		HasCompleted = true;
	}

	public void UpdateAchievement(PHPawn player, int updateProgress = 1)
	{
		AchProgress += updateProgress;

		Log.Info( $"{AchName} - {AchProgress}" );

		if ( AchProgress >= AchProgressMax )
			GiveAchievement( player );
	}

	public void GiveAchievement(PHPawn player, AchBase serverAch = null)
	{
		player.GiveCoins( AchCoinsReward );

		if( AchItemRewards.Count > 0 )
		{
			foreach ( var achItem in AchItemRewards )
				player.PHInventory.AddItem( achItem );
		}

		player.PlaySound( AchUnlockSound );
		player.AchList.Add( AchName );
		
		HasCompleted = true;

		player.AchChecker.Remove( this );

		ConsoleSystem.Run( "say", $"{player.Client.Name} has earned the achievement: {AchName}", true );

		Log.Info( $"{player.Client.Name} has earned the achievement {AchName}" );
	}
}

