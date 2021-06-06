using UnityEngine;

public class AvatarStateAttack_1UpLeft : AvatarStateAttack_1
{
	protected AvatarStateIdleUpLeft avatarStateIdleUpLeft;

	protected new void Awake()
	{
		base.Awake();

		avatarStateIdleUpLeft = GetComponent<AvatarStateIdleUpLeft>();
		if (avatarStateIdleUpLeft == null)
		{
			Debug.LogWarningFormat("{0} does not have a AvatarStateIdleUpLeft Component!", name);
		}
	}

	protected override void HandleAttack1InputBroadcasted(bool isPressing)
	{
		if (!isPressing)
		{
			SwitchToState(avatarStateIdleUpLeft);
		}
	}

	protected override void HandleAnimationEnded()
	{
		SwitchToState(avatarStateIdleUpLeft);
	}
}