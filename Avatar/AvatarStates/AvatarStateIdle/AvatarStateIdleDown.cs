using UnityEngine;

public class AvatarStateIdleDown : AvatarStateIdle
{
	protected AvatarStateRunDown avatarStateRunDown;
	protected AvatarStateAttack_1Down avatarStateAttack_1Down;

	protected new void Awake()
	{
		base.Awake();

		avatarStateRunDown = GetComponent<AvatarStateRunDown>();
		if (avatarStateRunDown == null)
		{
			Debug.LogWarningFormat("{0} does not have a AvatarStateRunDown Component!", name);
		}

		avatarStateAttack_1Down = GetComponent<AvatarStateAttack_1Down>();
		if (avatarStateAttack_1Down == null)
		{
			Debug.LogWarningFormat("{0} does not have a AvatarStateAttack_1Down Component!", name);
		}
	}

	protected override void HandleMovingStatusChanged(bool isMoving)
	{
		if (isMoving)
		{
			SwitchToState(avatarStateRunDown);
		}
	}

	protected override void SwitchToAttackState()
	{
		SwitchToState(avatarStateAttack_1Down);
	}
}