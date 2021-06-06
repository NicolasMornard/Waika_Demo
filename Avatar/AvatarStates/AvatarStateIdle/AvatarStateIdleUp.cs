using UnityEngine;

public class AvatarStateIdleUp : AvatarStateIdle
{
	protected AvatarStateRunUp avatarStateRunUp;
	protected AvatarStateAttack_1Up avatarStateAttack_1Up;

	protected new void Awake()
	{
		base.Awake();

		avatarStateRunUp = GetComponent<AvatarStateRunUp>();
		if (avatarStateRunUp == null)
		{
			Debug.LogWarningFormat("{0} does not have a AvatarStateRunUp Component!", name);
		}

		avatarStateAttack_1Up = GetComponent<AvatarStateAttack_1Up>();
		if (avatarStateAttack_1Up == null)
		{
			Debug.LogWarningFormat("{0} does not have a AvatarStateAttack_1Up Component!", name);
		}
	}

	protected override void HandleMovingStatusChanged(bool isMoving)
	{
		if (isMoving)
		{
			SwitchToState(avatarStateRunUp);
		}
	}

	protected override void SwitchToAttackState()
	{
		SwitchToState(avatarStateAttack_1Up);
	}
}