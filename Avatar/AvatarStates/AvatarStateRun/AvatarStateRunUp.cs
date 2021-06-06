using UnityEngine;

public class AvatarStateRunUp : AvatarStateRun
{
	protected AvatarStateIdleUp avatarStateIdleUp;
	protected AvatarStateAttack_1Up avatarStateAttack_1Up;

	protected new void Awake()
	{
		base.Awake();

		avatarStateIdleUp = GetComponent<AvatarStateIdleUp>();
		if (avatarStateIdleUp == null)
		{
			Debug.LogWarningFormat("{0} does not have a AvatarStateIdleUp Component!", name);
		}

		avatarStateAttack_1Up = GetComponent<AvatarStateAttack_1Up>();
		if (avatarStateAttack_1Up == null)
		{
			Debug.LogWarningFormat("{0} does not have a AvatarStateAttack_1Up Component!", name);
		}
	}

	protected override void HandleMovingStatusChanged(bool isMoving)
	{
		if (!isMoving)
		{
			SwitchToState(avatarStateIdleUp);
		}
	}

	protected override void SwitchToAttackState()
	{
		SwitchToState(avatarStateAttack_1Up);
	}
}