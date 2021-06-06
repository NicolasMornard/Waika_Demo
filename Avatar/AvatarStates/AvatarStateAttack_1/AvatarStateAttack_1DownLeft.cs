using UnityEngine;

public class AvatarStateAttack_1DownLeft : AvatarStateAttack_1
{
	protected AvatarStateIdleDownLeft avatarStateIdleDownLeft;

	protected new void Awake()
	{
		base.Awake();

		avatarStateIdleDownLeft = GetComponent<AvatarStateIdleDownLeft>();
		if (avatarStateIdleDownLeft == null)
		{
			Debug.LogWarningFormat("{0} does not have a AvatarStateIdleDownLeft Component!", name);
		}
	}

	protected override void HandleAttack1InputBroadcasted(bool isPressing)
	{
		if (!isPressing)
		{
			SwitchToState(avatarStateIdleDownLeft);
		}
	}

	protected override void HandleAnimationEnded()
	{
		SwitchToState(avatarStateIdleDownLeft);
	}
}