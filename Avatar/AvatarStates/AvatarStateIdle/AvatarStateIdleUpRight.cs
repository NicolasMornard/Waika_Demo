using UnityEngine;

public class AvatarStateIdleUpRight : AvatarStateIdle
{
	protected AvatarStateRunUpRight avatarStateRunUpRight;
	protected AvatarStateAttack_1UpRight avatarStateAttack_1UpRight;

	protected new void Awake()
	{
		base.Awake();

		avatarStateRunUpRight = GetComponent<AvatarStateRunUpRight>();
		if (avatarStateRunUpRight == null)
		{
			Debug.LogWarningFormat("{0} does not have a AvatarStateRunUpRight Component!", name);
		}

		avatarStateAttack_1UpRight = GetComponent<AvatarStateAttack_1UpRight>();
		if (avatarStateAttack_1UpRight == null)
		{
			Debug.LogWarningFormat("{0} does not have a AvatarStateAttack_1UpRight Component!", name);
		}
	}

	protected override void HandleMovingStatusChanged(bool isMoving)
	{
		if (isMoving)
		{
			SwitchToState(avatarStateRunUpRight);
		}
	}

	protected override void SwitchToAttackState()
	{
		SwitchToState(avatarStateAttack_1UpRight);
	}
}