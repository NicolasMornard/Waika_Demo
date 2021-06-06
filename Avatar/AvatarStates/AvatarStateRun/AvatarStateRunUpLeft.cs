using UnityEngine;

public class AvatarStateRunUpLeft : AvatarStateRun
{
	protected AvatarStateIdleUpLeft avatarStateIdleUpLeft;
	protected AvatarStateAttack_1UpLeft avatarStateAttack_1UpLeft;

	protected new void Awake()
	{
		base.Awake();

		avatarStateIdleUpLeft = GetComponent<AvatarStateIdleUpLeft>();
		if (avatarStateIdleUpLeft == null)
		{
			Debug.LogWarningFormat("{0} does not have a AvatarStateIdleUpLeft Component!", name);
		}

		avatarStateAttack_1UpLeft = GetComponent<AvatarStateAttack_1UpLeft>();
		if (avatarStateAttack_1UpLeft == null)
		{
			Debug.LogWarningFormat("{0} does not have a AvatarStateAttack_1UpLeft Component!", name);
		}
	}

	protected override void HandleMovingStatusChanged(bool isMoving)
	{
		if (!isMoving)
		{
			SwitchToState(avatarStateIdleUpLeft);
		}
	}

	protected override void SwitchToAttackState()
	{
		SwitchToState(avatarStateAttack_1UpLeft);
	}
}