using UnityEngine;

public class AvatarStateRunDown : AvatarStateRun
{
	protected AvatarStateIdleDown avatarStateIdleDown;
	protected AvatarStateAttack_1Down avatarStateAttack_1Down;

	protected new void Awake()
	{
		base.Awake();

		avatarStateIdleDown = GetComponent<AvatarStateIdleDown>();
		if (avatarStateIdleDown == null)
		{
			Debug.LogWarningFormat("{0} does not have a AvatarStateIdleDown Component!", name);
		}

		avatarStateAttack_1Down = GetComponent<AvatarStateAttack_1Down>();
		if (avatarStateIdleDown == null)
		{
			Debug.LogWarningFormat("{0} does not have a AvatarStateAttack_1Down Component!", name);
		}
	}

	protected override void HandleMovingStatusChanged(bool isMoving)
	{
		if (!isMoving)
		{
			SwitchToState(avatarStateIdleDown);
		}
	}

	protected override void SwitchToAttackState()
	{
		SwitchToState(avatarStateAttack_1Down);
	}
}