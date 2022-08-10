using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;

public class AchTracker
{
	public IList<AchBase> Tracked;

	public AchTracker()
	{
		Tracked = new List<AchBase>();
	}

	public void Start(string achToStart)
	{
		//Checks if we made a mistake on the achievement name
		if(TypeLibrary.GetTypeByName<AchBase>(achToStart) == null)
		{
			Log.Error( "Invalid achievement name to start" );
			return;
		}

		//Checks if we are already tracking this achievement
		if ( Tracked.FirstOrDefault( x => x.ToString() == achToStart ) != null )
			return;

		//TODO: check if the player has completed the achievement already so we don't restart it

		AchBase newAch = TypeLibrary.Create<AchBase>( achToStart );
		newAch.IsCompleted = false;
		newAch.Progress = 0;

		Tracked.Add(newAch);
	}

	public void Update( BasePawn player, string achName, int updateProg = 1 )
	{
		if(!CheckAchievement(achName))
			Start( achName );

		AchBase ach = Tracked.FirstOrDefault( x => x.ToString() == achName );

		if ( ach.IsCompleted )
			return;

		ach.Progress += updateProg;

		if ( ach.Progress >= ach.AchGoal )
			GiveRewards( player, ach );
	}

	public bool CheckAchievement(string achName)
	{
		if ( Tracked.FirstOrDefault( x => x.ToString() == achName ) == null)
			return false;

		return true;
	}
	public void GiveRewards(BasePawn player, AchBase ach)
	{
		player.PlaySound( "ach_award" );
		ach.IsCompleted = true;
	}
}
