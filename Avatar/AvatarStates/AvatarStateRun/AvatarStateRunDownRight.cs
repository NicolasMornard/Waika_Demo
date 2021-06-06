using UnityEngine;

public class AvatarStateRunDownRight : AvatarStateRun
{
	protected AvatarStateIdleDownRight avatarStateIdleDownRight;
	protected AvatarStateAttack_1DownRight avatarStateAttack_1DownRight;

	protected new void Awake()
	{
		base.Awake();

		avatarStateIdleDownRight = GetComponent<AvatarStateIdleDownRight>();
		if (avatarStateIdleDownRight == null)
		{
			Debug.LogWarningFormat("{0} does not have a AvatarStateIdleDownRight Component!", name);
		}

		avatarStateAttack_1DownRight = GetComponent<AvatarStateAttack_1DownRight>();
		if (avatarStateAttack_1DownRight == null)
		{
			Debug.LogWarningFormat("{0} does not have a AvatarStateAttack_1DownRight Component!", name);
		}
	}

	protected override void HandleMovingStatusChanged(bool isMoving)
	{
		if (!isMoving)
		{
			SwitchToState(avatarStateIdleDownRight);
		}
	}

	protected override void SwitchToAttackState()
	{
		SwitchToState(avatarStateAttack_1DownRight);
	}
}