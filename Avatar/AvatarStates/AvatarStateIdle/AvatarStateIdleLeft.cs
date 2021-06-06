using UnityEngine;

public class AvatarStateIdleLeft : AvatarStateIdle
{
	protected AvatarStateRunLeft avatarStateRunLeft;
	protected AvatarStateAttack_1Left avatarStateAttack_1Left;

	protected new void Awake()
	{
		base.Awake();

		avatarStateRunLeft = GetComponent<AvatarStateRunLeft>();
		if (avatarStateRunLeft == null)
		{
			Debug.LogWarningFormat("{0} does not have a AvatarStateRunLeft Component!", name);
		}

		avatarStateAttack_1Left = GetComponent<AvatarStateAttack_1Left>();
		if (avatarStateAttack_1Left == null)
		{
			Debug.LogWarningFormat("{0} does not have a AvatarStateAttack_1Right Component!", name);
		}
	}

	protected override void HandleMovingStatusChanged(bool isMoving)
	{
		if (isMoving)
		{
			SwitchToState(avatarStateRunLeft);
		}
	}

	protected override void SwitchToAttackState()
	{
		SwitchToState(avatarStateAttack_1Left);
	}
}