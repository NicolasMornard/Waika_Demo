using UnityEngine;

public class AvatarStateAttack_1DownRight : AvatarStateAttack_1
{
	protected AvatarStateIdleDownRight avatarStateIdleDownRight;

	protected new void Awake()
	{
		base.Awake();

		avatarStateIdleDownRight = GetComponent<AvatarStateIdleDownRight>();
		if (avatarStateIdleDownRight == null)
		{
			Debug.LogWarningFormat("{0} does not have a AvatarStateIdleDownRight Component!", name);
		}
	}

	protected override void HandleAttack1InputBroadcasted(bool isPressing)
	{
		if (!isPressing)
		{
			SwitchToState(avatarStateIdleDownRight);
		}
	}

	protected override void HandleAnimationEnded()
	{
		SwitchToState(avatarStateIdleDownRight);
	}
}