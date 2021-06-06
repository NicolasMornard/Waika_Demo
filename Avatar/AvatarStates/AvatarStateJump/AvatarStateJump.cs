using UnityEngine;

using static AvatarPlayer;

public class AvatarStateJump : AvatarStateBase
{
	[SerializeField] private float maxJumpHeight = 0.8f;

	public bool HasReachedMaxJumpingHeight
	{
		get
		{
			if (avatar3DMovement.IsGrounded)
			{
				return false;
			}

			return CurrentJumpHeight >= maxJumpHeight;
		}
	}
	public float CurrentJumpHeight
	{
		get
		{
			if (avatar3DMovement.IsGrounded)
			{
				return 0.0f;
			}

			return transform.position.y - heightAtJumpStart;
		}
	}
	protected override void UpdateStateWithRotation(AvatarOrientation orientation)
	{
		//TODO
	}

	protected override void HandleJumpingInputBroadcasted(bool isPressing)
	{
		if (isPressing)
		{
			if (avatar.CanJump)
			{
				if (!HasReachedMaxJumpingHeight)
				{
					Jump();
				}
				else
				{
					avatar.SetCanJump(false);
				}
			}
		}
		else if (avatar.CanJump)
		{
			avatar.SetCanJump(false);
		}
	}
	protected override void HandleMovingStatusChanged(bool isMoving)
	{
		//TODO
	}

	protected override void HandleGroundedStatusChanged(bool isGrounded)
	{
		if (isGrounded)
		{
			avatar.SetCanJump(false);
			SwitchToState(avatarStateRun);
		}
	}
}