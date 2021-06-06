using UnityEngine;

public class AvatarStateAttack_1Right : AvatarStateAttack_1
{
	protected AvatarStateIdleRight avatarStateIdleRight;

	protected new void Awake()
	{
		base.Awake();

		avatarStateIdleRight = GetComponent<AvatarStateIdleRight>();
		if (avatarStateIdleRight == null)
		{
			Debug.LogWarningFormat("{0} does not have a AvatarStateIdleRight Component!", name);
		}
	}

	protected override void HandleAttack1InputBroadcasted(bool isPressing)
	{
		if (!isPressing)
		{
			SwitchToState(avatarStateIdleRight);
		}
	}

	protected override void HandleAnimationEnded()
	{
		SwitchToState(avatarStateIdleRight);
	}
}