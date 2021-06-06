using UnityEngine;

public class AvatarStateAttack_1Down : AvatarStateAttack_1
{
	protected AvatarStateIdleDown avatarStateIdleDown;

	protected new void Awake()
	{
		base.Awake();

		avatarStateIdleDown = GetComponent<AvatarStateIdleDown>();
		if (avatarStateIdleDown == null)
		{
			Debug.LogWarningFormat("{0} does not have a AvatarStateIdleDown Component!", name);
		}
	}

	protected override void HandleAttack1InputBroadcasted(bool isPressing)
	{
		if (!isPressing)
		{
			SwitchToState(avatarStateIdleDown);
		}
	}

	protected override void HandleAnimationEnded()
	{
		SwitchToState(avatarStateIdleDown);
	}
}