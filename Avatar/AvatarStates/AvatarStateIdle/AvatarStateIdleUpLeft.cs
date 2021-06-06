using UnityEngine;

public class AvatarStateIdleUpLeft : AvatarStateIdle
{
	protected AvatarStateRunUpLeft avatarStateRunUpLeft;
	protected AvatarStateAttack_1UpLeft avatarStateAttack_1UpLeft;

	protected new void Awake()
	{
		base.Awake();

		avatarStateRunUpLeft = GetComponent<AvatarStateRunUpLeft>();
		if (avatarStateRunUpLeft == null)
		{
			Debug.LogWarningFormat("{0} does not have a AvatarStateRunUpLeft Component!", name);
		}

		avatarStateAttack_1UpLeft = GetComponent<AvatarStateAttack_1UpLeft>();
		if (avatarStateAttack_1UpLeft == null)
		{
			Debug.LogWarningFormat("{0} does not have a AvatarStateAttack_1UpLeft Component!", name);
		}
	}

	protected override void HandleMovingStatusChanged(bool isMoving)
	{
		if (isMoving)
		{
			SwitchToState(avatarStateRunUpLeft);
		}
	}

	protected override void SwitchToAttackState()
	{
		SwitchToState(avatarStateAttack_1UpLeft);
	}
}