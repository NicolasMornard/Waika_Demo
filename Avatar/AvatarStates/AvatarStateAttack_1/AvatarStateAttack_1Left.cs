using UnityEngine;

public class AvatarStateAttack_1Left : AvatarStateAttack_1
{
	protected AvatarStateIdleLeft avatarStateIdleLeft;

	protected new void Awake()
	{
		base.Awake();

		avatarStateIdleLeft = GetComponent<AvatarStateIdleLeft>();
		if (avatarStateIdleLeft == null)
		{
			Debug.LogWarningFormat("{0} does not have a AvatarStateIdleLeft Component!", name);
		}
	}

	protected override void HandleAttack1InputBroadcasted(bool isPressing)
	{
		if (!isPressing)
		{
			SwitchToState(avatarStateIdleLeft);
		}
	}

	protected override void HandleAnimationEnded()
	{
		SwitchToState(avatarStateIdleLeft);
	}
}