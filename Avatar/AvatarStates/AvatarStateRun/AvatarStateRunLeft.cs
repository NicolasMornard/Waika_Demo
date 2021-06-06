using UnityEngine;

public class AvatarStateRunLeft : AvatarStateRun
{
	protected AvatarStateIdleLeft avatarStateIdleLeft;
	protected AvatarStateAttack_1Left avatarStateAttack_1Left;

	protected new void Awake()
	{
		base.Awake();

		avatarStateIdleLeft = GetComponent<AvatarStateIdleLeft>();
		if (avatarStateIdleLeft == null)
		{
			Debug.LogWarningFormat("{0} does not have a AvatarStateIdleLeft Component!", name);
		}

		avatarStateAttack_1Left = GetComponent<AvatarStateAttack_1Left>();
		if (avatarStateAttack_1Left == null)
		{
			Debug.LogWarningFormat("{0} does not have a AvatarStateAttack_1Left Component!", name);
		}
	}

	protected override void HandleMovingStatusChanged(bool isMoving)
	{
		if (!isMoving)
		{
			SwitchToState(avatarStateIdleLeft);
		}
	}

	protected override void SwitchToAttackState()
	{
		SwitchToState(avatarStateAttack_1Left);
	}
}