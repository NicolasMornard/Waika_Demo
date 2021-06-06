using UnityEngine;

public class AvatarStateRunRight : AvatarStateRun
{
	protected AvatarStateIdleRight avatarStateIdleRight;
	protected AvatarStateAttack_1Right avatarStateAttack_1Right;

	protected new void Awake()
	{
		base.Awake();

		avatarStateIdleRight = GetComponent<AvatarStateIdleRight>();
		if (avatarStateIdleRight == null)
		{
			Debug.LogWarningFormat("{0} does not have a AvatarStateIdleRight Component!", name);
		}

		avatarStateAttack_1Right = GetComponent<AvatarStateAttack_1Right>();
		if (avatarStateAttack_1Right == null)
		{
			Debug.LogWarningFormat("{0} does not have a AvatarStateAttack_1Right Component!", name);
		}
	}

	protected override void HandleMovingStatusChanged(bool isMoving)
	{
		if (!isMoving)
		{
			SwitchToState(avatarStateIdleRight);
		}
	}
	
	protected override void SwitchToAttackState()
	{
		SwitchToState(avatarStateAttack_1Right);
	}
}