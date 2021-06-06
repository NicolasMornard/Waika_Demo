using UnityEngine;

using static AvatarPlayer;

public class AvatarStateBase : MonoBehaviour
{
	[SerializeField] protected AnimationSprites animationSprites;

	protected float heightAtJumpStart = 0.0f;

	protected AvatarPlayer avatar;
	protected Avatar3DMovement avatar3DMovement;
	protected Avatar3DAnimations animations;
	protected AvatarStateManager avatarStateManager;

	protected AvatarStateIdle avatarStateIdle;
	protected AvatarStateRun avatarStateRun;
	protected AvatarStateJump avatarStateJump;

	protected void Awake()
	{
		avatar = GetComponentInParent<AvatarPlayer>();
		if (avatar == null)
		{
			Debug.LogFormat("{0} does not have an AvatarPlayer component!");
		}

		avatar3DMovement = GetComponentInParent<Avatar3DMovement>();
		if (avatar3DMovement == null)
		{
			Debug.LogFormat("{0} does not have an Avatar3DMovement component!");
		}

		animations = transform.parent.GetComponentInChildren<Avatar3DAnimations>();
		if (animations == null)
		{
			Debug.LogErrorFormat("{0} does not have a Avatar3DAnimations Component!");
		}

		avatarStateManager = transform.parent.GetComponentInChildren<AvatarStateManager>();
		if (avatarStateManager == null)
		{
			Debug.LogErrorFormat("{0} does not have a AvatarStateManager Component!");
		}

		avatarStateIdle = GetComponent<AvatarStateIdle>();
		if (avatarStateIdle == null)
		{
			Debug.LogWarningFormat("{0} does not have a AvatarStateIdle Component!", name);
		}

		avatarStateRun = GetComponent<AvatarStateRun>();
		if (avatarStateRun == null)
		{
			Debug.LogWarningFormat("{0} does not have a AvatarStateRun Component!", name);
		}

		avatarStateJump = GetComponent<AvatarStateJump>();
		if (avatarStateJump == null)
		{
			Debug.LogWarningFormat("{0} does not have a AvatarStateJump Component!", name);
		}
	}

	protected void SwitchToState(AvatarStateBase stateToSwitchTo)
	{
		if (stateToSwitchTo == null)
		{
			Debug.LogError("Trying to switch to a null State!");
			return;
		}

		stateToSwitchTo.enabled = true;

		ExitState();
	}

	protected void MoveInDirection(Vector3 direction)
	{
		avatar3DMovement.MoveInDirection(direction);
	}

	protected void Jump()
	{
		avatar3DMovement.Jump();
	}

	protected virtual void EnterState()
	{
	}

	protected virtual void UpdateStateWithRotation(AvatarOrientation orientation)
	{
	}
	protected virtual void HandleMovingStatusChanged(bool isMoving)
	{
	}

	protected virtual void HandleGroundedStatusChanged(bool isGrounded)
	{
	}

	protected virtual void HandleAnimationEnded()
	{
	}

	protected virtual void HandleJumpingInputBroadcasted(bool isPressing)
	{
	}

	protected virtual void HandleAttack1InputBroadcasted(bool isPressing)
	{
	}

	protected void ExitState()
	{
		enabled = false;
	}

	private void SubscribeToDelegates()
	{
		avatar3DMovement.OnOrientationChange += OnOrientationChanged;
		avatar3DMovement.OnMovingStatusChange += OnMovingStatusChanged;
		avatar3DMovement.OnGroundedStatusChange += OnGroundedStatusChanged;

		animations.OnAnimationEnds += OnAnimationEnded;

		GameDirector.GameDirectorInstance.MovingInputBroadcast += OnMovingInputBroadcasted;
		GameDirector.GameDirectorInstance.JumpingInputBroadcast += OnJumpingInputBroadcasted;
		GameDirector.GameDirectorInstance.Attack1InputBroadcast += OnAttack1InputBroadcasted;
	}

	private void UnsubscribeFromDelegates()
	{
		avatar3DMovement.OnOrientationChange -= OnOrientationChanged;
		avatar3DMovement.OnMovingStatusChange -= OnMovingStatusChanged;
		avatar3DMovement.OnGroundedStatusChange -= OnGroundedStatusChanged;
		avatar3DMovement.OnGroundedStatusChange -= OnGroundedStatusChanged;

		animations.OnAnimationEnds -= OnAnimationEnded;

		GameDirector.GameDirectorInstance.MovingInputBroadcast -= OnMovingInputBroadcasted;
		GameDirector.GameDirectorInstance.JumpingInputBroadcast -= OnJumpingInputBroadcasted;
		GameDirector.GameDirectorInstance.Attack1InputBroadcast -= OnAttack1InputBroadcasted;
	}

	protected void HandleMovingInput(MovingInput movingInput)
	{
		if (this is AvatarStateAttack_1)
		{
			return;
		}

		if (movingInput.Pressed)
		{
			MoveInDirection(new Vector3(
				movingInput.HorizontalValue,
				0.0f,
				movingInput.VerticalValue).normalized);
		}
	}

	private void OnOrientationChanged(AvatarOrientation orientation)
	{
		UpdateStateWithRotation(orientation);
	}

	private void OnMovingStatusChanged(bool isMoving)
	{
		HandleMovingStatusChanged(isMoving);
	}

	private void OnGroundedStatusChanged(bool isGrounded)
	{
		HandleGroundedStatusChanged(isGrounded);
	}

	private void OnAnimationEnded()
	{
		HandleAnimationEnded();
	}

	private void OnMovingInputBroadcasted(MovingInput movingInput)
	{
		HandleMovingInput(movingInput);
	}

	private void OnJumpingInputBroadcasted(bool isJumping)
	{
		HandleJumpingInputBroadcasted(isJumping);
	}

	private void OnAttack1InputBroadcasted(bool isPressing)
	{
		HandleAttack1InputBroadcasted(isPressing);
	}

	private void OnEnable()
	{
		SubscribeToDelegates();
		EnterState();
	}

	private void OnDisable()
	{
		UnsubscribeFromDelegates();
		StopAllCoroutines();
	}
}