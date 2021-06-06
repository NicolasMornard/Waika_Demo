using UnityEngine;

using static AvatarPlayer;

public class AvatarStateRun : AvatarStateBase
{
	protected AvatarStateRunRight avatarStateRunRight;
	protected AvatarStateRunLeft avatarStateRunLeft;

	protected AvatarStateRunUp avatarStateRunUp;
	protected AvatarStateRunUpRight avatarStateRunUpRight;
	protected AvatarStateRunUpLeft avatarStateRunUpLeft;

	protected AvatarStateRunDown avatarStateRunDown;
	protected AvatarStateRunDownRight avatarStateRunDownRight;
	protected AvatarStateRunDownLeft avatarStateRunDownLeft;

	protected new void Awake()
	{
		base.Awake();

		avatarStateRunRight = GetComponent<AvatarStateRunRight>();
		if (avatarStateRunRight == null)
		{
			Debug.LogWarningFormat("{0} does not have a AvatarStateRunRight Component!", name);
		}

		avatarStateRunLeft = GetComponent<AvatarStateRunLeft>();
		if (avatarStateRunLeft == null)
		{
			Debug.LogWarningFormat("{0} does not have a AvatarStateRunLeft Component!", name);
		}

		avatarStateRunUp = GetComponent<AvatarStateRunUp>();
		if (avatarStateRunUp == null)
		{
			Debug.LogWarningFormat("{0} does not have a AvatarStateRunUp Component!", name);
		}

		avatarStateRunDown = GetComponent<AvatarStateRunDown>();
		if (avatarStateRunDown == null)
		{
			Debug.LogWarningFormat("{0} does not have a AvatarStateRunDown Component!", name);
		}

		avatarStateRunDownRight = GetComponent<AvatarStateRunDownRight>();
		if (avatarStateRunDownRight == null)
		{
			Debug.LogWarningFormat("{0} does not have a AvatarStateRunDownRight Component!", name);
		}

		avatarStateRunDownLeft = GetComponent<AvatarStateRunDownLeft>();
		if (avatarStateRunDownLeft == null)
		{
			Debug.LogWarningFormat("{0} does not have a AvatarStateRunDownLeft Component!", name);
		}

		avatarStateRunUpRight = GetComponent<AvatarStateRunUpRight>();
		if (avatarStateRunRight == null)
		{
			Debug.LogWarningFormat("{0} does not have a AvatarStateRunUpRight Component!", name);
		}

		avatarStateRunUpLeft = GetComponent<AvatarStateRunUpLeft>();
		if (avatarStateRunUpLeft == null)
		{
			Debug.LogWarningFormat("{0} does not have a AvatarStateRunUpLeft Component!", name);
		}

	}

	protected override void EnterState()
	{
		animations.LaunchAnimation(animationSprites);
	}

	protected override void UpdateStateWithRotation(AvatarOrientation orientation)
	{
		switch (orientation)
		{
			case AvatarOrientation.Up:
				SwitchToState(avatarStateRunUp);
				break;
			case AvatarOrientation.Down:
				SwitchToState(avatarStateRunDown);
				break;
			case AvatarOrientation.Right:
				SwitchToState(avatarStateRunRight);
				break;
			case AvatarOrientation.Left:
				SwitchToState(avatarStateRunLeft);
				break;
			case AvatarOrientation.DownLeft:
				SwitchToState(avatarStateRunDownLeft);
				break;
			case AvatarOrientation.DownRight:
				SwitchToState(avatarStateRunDownRight);
				break;
			case AvatarOrientation.UpRight:
				SwitchToState(avatarStateRunUpRight);
				break;
			case AvatarOrientation.UpLeft:
				SwitchToState(avatarStateRunUpLeft);
				break;
		}
	}

	protected override void HandleJumpingInputBroadcasted(bool isPressing)
	{
		if (isPressing)
		{
			if (avatar.CanJump)
			{
				Jump();
			}
		}
		else if (!avatar.CanJump)
		{
			avatar.SetCanJump(true);
		}
	}

	protected override void HandleMovingStatusChanged(bool isMoving)
	{
		if (!isMoving)

		{
			SwitchToState(avatarStateIdle);
		}
	}

	protected override void HandleGroundedStatusChanged(bool isGrounded)
	{
		if (!isGrounded)
		{
			SwitchToState(avatarStateJump);
		}
	}
	protected override void HandleAttack1InputBroadcasted(bool isPressing)
	{
		if (isPressing)
		{
			if (avatarStateManager.CanAttack)
			{
				avatarStateManager.CanAttack = false;
				SwitchToAttackState();
			}
		}
		else if (!avatarStateManager.CanAttack)
		{
			avatarStateManager.CanAttack = true;
		}
	}

	protected virtual void SwitchToAttackState()
	{
	}
}