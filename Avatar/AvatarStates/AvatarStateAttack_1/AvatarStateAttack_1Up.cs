using UnityEngine;

public class AvatarStateAttack_1Up : AvatarStateAttack_1
{
	protected AvatarStateIdleUp avatarStateIdleUp;

	protected new void Awake()
	{
		base.Awake();

		avatarStateIdleUp = GetComponent<AvatarStateIdleUp>();
		if (avatarStateIdleUp == null)
		{
			Debug.LogWarningFormat("{0} does not have a AvatarStateIdleUp Component!", name);
		}
	}

	protected override void HandleAttack1InputBroadcasted(bool isPressing)
	{
		if (!isPressing)
		{
			SwitchToState(avatarStateIdleUp);
		}
	}

	protected override void HandleAnimationEnded()
	{
		SwitchToState(avatarStateIdleUp);
	}
}