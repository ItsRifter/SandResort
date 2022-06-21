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

	public virtual int AchProgress { get; set; } = 0;

	//Updates the achievement
	public void UpdateAchievement(LobbyPawn player, int updateProgress = 1)
	{
		AchProgress += updateProgress;

		if ( AchProgress >= AchProgressMax )
			GiveAchievement( player );
	}

	//Upon progress reached to max, give the achievement to the player and announce it
	public void GiveAchievement(LobbyPawn player)
	{
		player.GiveCoins( AchCoinsReward );

		//If there are specific rewards, give them to the player
		if( AchItemRewards.Count > 0 )
		{
			foreach ( var achItem in AchItemRewards )
				player.PHInventory.AddItem( achItem );
		}

		player.PlaySound( AchUnlockSound );
		player.AchList.Add( this );
		
		HasCompleted = true;

		player.AchChecker.Remove( this );

		//ConsoleSystem.Run( "say", $"{player.Client.Name} has earned the achievement: {AchName}", true );

		Log.Info( $"{player.Client.Name} has earned the achievement {AchName}" );
	}
}

