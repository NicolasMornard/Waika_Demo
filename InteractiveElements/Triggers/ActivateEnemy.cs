using UnityEngine;

public class ActivateEnemy : ElementTrigger
{
	public Enemy EnemyToActivate;
	public Action ActionToActivate = Action.FollowWaypoint;

	// Enum
	public enum Action
	{
		FollowWaypoint,
		SearchAvatar, // TODO
		FightAvatar, // TODO
	};

	protected override void TriggerAction()
	{
		switch (ActionToActivate)
		{
			case Action.FollowWaypoint: // TODO
				EnemyToActivate.SetGeneralState(Enemy.EnemyState.FollowPath);
				break;
			case Action.SearchAvatar: // TODO
				EnemyToActivate.SetGeneralState(Enemy.EnemyState.Search);
				break;
			case Action.FightAvatar: // TODO
				EnemyToActivate.SetGeneralState(Enemy.EnemyState.Search);
				break;
		}
	}
}
