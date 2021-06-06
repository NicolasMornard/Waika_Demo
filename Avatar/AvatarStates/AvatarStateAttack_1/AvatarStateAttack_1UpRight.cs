using UnityEngine;

public class AvatarStateAttack_1UpRight : AvatarStateAttack_1
{
	protected AvatarStateIdleUpRight avatarStateIdleUpRight;

	protected new void Awake()
	{
		base.Awake();

		avatarStateIdleUpRight = GetComponent<AvatarStateIdleUpRight>();
		if (avatarStateIdleUpRight == null)
		{
			Debug.LogWarningFormat("{0} does not have a AvatarStateIdleUpRight Component!", name);
		}
	}

	protected override void HandleAttack1InputBroadcasted(bool isPressing)
	{
		if (!isPressing)
		{
			SwitchToState(avatarStateIdleUpRight);
		}
	}

	protected override void HandleAnimationEnded()
	{
		SwitchToState(avatarStateIdleUpRight);
	}
}