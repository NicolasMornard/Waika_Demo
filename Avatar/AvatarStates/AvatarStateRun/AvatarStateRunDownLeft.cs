using UnityEngine;

public class AvatarStateRunDownLeft : AvatarStateRun
{
	protected AvatarStateIdleDownLeft avatarStateIdleDownLeft;
	protected AvatarStateAttack_1DownLeft avatarStateAttack_1DownLeft;

	protected new void Awake()
	{
		base.Awake();

		avatarStateIdleDownLeft = GetComponent<AvatarStateIdleDownLeft>();
		if (avatarStateIdleDownLeft == null)
		{
			Debug.LogWarningFormat("{0} does not have a AvatarStateIdleDownLeft Component!", name);
		}

		avatarStateAttack_1DownLeft = GetComponent<AvatarStateAttack_1DownLeft>();
		if (avatarStateAttack_1DownLeft == null)
		{
			Debug.LogWarningFormat("{0} does not have a AvatarStateAttack_1DownLeft Component!", name);
		}
	}

	protected override void HandleMovingStatusChanged(bool isMoving)
	{
		if (!isMoving)
		{
			SwitchToState(avatarStateIdleDownLeft);
		}
	}

	protected override void SwitchToAttackState()
	{
		SwitchToState(avatarStateAttack_1DownLeft);
	}
}