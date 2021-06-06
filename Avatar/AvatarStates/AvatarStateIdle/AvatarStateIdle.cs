using UnityEngine;

using static AvatarPlayer;

public class AvatarStateIdle : AvatarStateBase
{
	protected AvatarStateIdleRight avatarStateIdleRight;
	protected AvatarStateIdleLeft avatarStateIdleLeft;

	protected AvatarStateIdleUp avatarStateIdleUp;
	protected AvatarStateIdleUpRight avatarStateIdleUpRight;
	protected AvatarStateIdleUpLeft avatarStateIdleUpLeft;

	protected AvatarStateIdleDown avatarStateIdleDown;
	protected AvatarStateIdleDownRight avatarStateIdleDownRight;
	protected AvatarStateIdleDownLeft avatarStateIdleDownLeft;

	protected new void Awake()
	{
		base.Awake();

		avatarStateIdleRight = GetComponent<AvatarStateIdleRight>();
		if (avatarStateIdleRight == null)
		{
			Debug.LogWarningFormat("{0} does not have a AvatarStateIdleRight Component!", name);
		}

		avatarStateIdleLeft = GetComponent<AvatarStateIdleLeft>();
		if (avatarStateIdleLeft == null)
		{
			Debug.LogWarningFormat("{0} does not have a AvatarStateIdleLeft Component!", name);
		}

		avatarStateIdleUp = GetComponent<AvatarStateIdleUp>();
		if (avatarStateIdleUp == null)
		{
			Debug.LogWarningFormat("{0} does not have a AvatarStateIdleUp Component!", name);
		}

		avatarStateIdleDown = GetComponent<AvatarStateIdleDown>();
		if (avatarStateIdleDown == null)
		{
			Debug.LogWarningFormat("{0} does not have a AvatarStateIdleDown Component!", name);
		}

		avatarStateIdleUpRight = GetComponent<AvatarStateIdleUpRight>();
		if (avatarStateIdleUpRight == null)
		{
			Debug.LogWarningFormat("{0} does not have a AvatarStateIdleUpRight Component!", name);
		}

		avatarStateIdleUpLeft = GetComponent<AvatarStateIdleUpLeft>();
		if (avatarStateIdleUpLeft == null)
		{
			Debug.LogWarningFormat("{0} does not have a AvatarStateIdleUpLeft Component!", name);
		}

		avatarStateIdleDownRight = GetComponent<AvatarStateIdleDownRight>();
		if (avatarStateIdleDownRight == null)
		{
			Debug.LogWarningFormat("{0} does not have a AvatarStateIdleDownRight Component!", name);
		}

		avatarStateIdleDownLeft = GetComponent<AvatarStateIdleDownLeft>();
		if (avatarStateIdleDownLeft == null)
		{
			Debug.LogWarningFormat("{0} does not have a AvatarStateIdleDownLeft Component!", name);
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
				SwitchToState(avatarStateIdleUp);
				break;
			case AvatarOrientation.Down:
				SwitchToState(avatarStateIdleDown);
				break;
			case AvatarOrientation.Right:
				SwitchToState(avatarStateIdleRight);
				break;
			case AvatarOrientation.Left:
				SwitchToState(avatarStateIdleLeft);
				break;
			case AvatarOrientation.DownLeft:
				SwitchToState(avatarStateIdleDownLeft);
				break;
			case AvatarOrientation.DownRight:
				SwitchToState(avatarStateIdleDownRight);
				break;
			case AvatarOrientation.UpRight:
				SwitchToState(avatarStateIdleUpRight);
				break;
			case AvatarOrientation.UpLeft:
				SwitchToState(avatarStateIdleUpLeft);
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