using UnityEngine;

public class AvatarStateIdleDownRight : AvatarStateIdle
{
	protected AvatarStateRunDownRight avatarStateRunDownRight;
	protected AvatarStateAttack_1DownRight avatarStateAttack_1DownRight;

	protected new void Awake()
	{
		base.Awake();

		avatarStateRunDownRight = GetComponent<AvatarStateRunDownRight>();
		if (avatarStateRunDownRight == null)
		{
			Debug.LogWarningFormat("{0} does not have a AvatarStateRunDownRight Component!", name);
		}

		avatarStateAttack_1DownRight = GetComponent<AvatarStateAttack_1DownRight>();
		if (avatarStateAttack_1DownRight == null)
		{
			Debug.LogWarningFormat("{0} does not have a AvatarStateAttack_1DownRight Component!", name);
		}
	}

	protected override void HandleMovingStatusChanged(bool isMoving)
	{
		if (isMoving)
		{
			SwitchToState(avatarStateRunDownRight);
		}
	}

	protected override void SwitchToAttackState()
	{
		SwitchToState(avatarStateAttack_1DownRight);
	}
}