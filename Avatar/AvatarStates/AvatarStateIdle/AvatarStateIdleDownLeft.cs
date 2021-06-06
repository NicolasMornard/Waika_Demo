using UnityEngine;

public class AvatarStateIdleDownLeft : AvatarStateIdle
{
	protected AvatarStateRunDownLeft avatarStateRunDownLeft;
	protected AvatarStateAttack_1DownLeft avatarStateAttack_1DownLeft;

	protected new void Awake()
	{
		base.Awake();

		avatarStateRunDownLeft = GetComponent<AvatarStateRunDownLeft>();
		if (avatarStateRunDownLeft == null)
		{
			Debug.LogWarningFormat("{0} does not have a AvatarStateRunDownLeft Component!", name);
		}

		avatarStateAttack_1DownLeft = GetComponent<AvatarStateAttack_1DownLeft>();
		if (avatarStateAttack_1DownLeft == null)
		{
			Debug.LogWarningFormat("{0} does not have a AvatarStateAttack_1DownLeft Component!", name);
		}
	}

	protected override void HandleMovingStatusChanged(bool isMoving)
	{
		if (isMoving)
		{
			SwitchToState(avatarStateRunDownLeft);
		}
	}

	protected override void SwitchToAttackState()
	{
		SwitchToState(avatarStateAttack_1DownLeft);
	}
}