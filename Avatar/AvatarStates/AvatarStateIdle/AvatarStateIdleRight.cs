using UnityEngine;

public class AvatarStateIdleRight : AvatarStateIdle
{
	protected AvatarStateRunRight avatarStateRunRight;
	protected AvatarStateAttack_1Right avatarStateAttack_1Right;

	protected new void Awake()
	{
		base.Awake();

		avatarStateRunRight = GetComponent<AvatarStateRunRight>();
		if (avatarStateRunRight == null)
		{
			Debug.LogWarningFormat("{0} does not have a AvatarStateRunRight Component!", name);
		}

		avatarStateAttack_1Right = GetComponent<AvatarStateAttack_1Right>();
		if (avatarStateAttack_1Right == null)
		{
			Debug.LogWarningFormat("{0} does not have a AvatarStateAttack_1Right Component!", name);
		}
	}

	protected override void HandleMovingStatusChanged(bool isMoving)
	{
		if (isMoving)
		{
			SwitchToState(avatarStateRunRight);
		}
	}

	protected override void SwitchToAttackState()
	{
		SwitchToState(avatarStateAttack_1Right);
	}
}