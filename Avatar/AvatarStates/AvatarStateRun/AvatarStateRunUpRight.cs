using UnityEngine;

public class AvatarStateRunUpRight : AvatarStateRun
{
	protected AvatarStateIdleUpRight avatarStateIdleUpRight;
	protected AvatarStateAttack_1UpRight avatarStateAttack_1UpRight;

	protected new void Awake()
	{
		base.Awake();

		avatarStateIdleUpRight = GetComponent<AvatarStateIdleUpRight>();
		if (avatarStateIdleUpRight == null)
		{
			Debug.LogWarningFormat("{0} does not have a AvatarStateIdleUpRight Component!", name);
		}

		avatarStateAttack_1UpRight = GetComponent<AvatarStateAttack_1UpRight>();
		if (avatarStateAttack_1UpRight == null)
		{
			Debug.LogWarningFormat("{0} does not have a AvatarStateAttack_1UpRight Component!", name);
		}
	}

	protected override void HandleMovingStatusChanged(bool isMoving)
	{
		if (!isMoving)
		{
			SwitchToState(avatarStateIdleUpRight);
		}
	}

	protected override void SwitchToAttackState()
	{
		SwitchToState(avatarStateAttack_1UpRight);
	}
}