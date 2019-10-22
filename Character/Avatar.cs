using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

using static GamePlayManager;
using static PlayerInput;
using static ObjectStates;

public class Avatar : Character
{
	// Public - Inspector
	public float Dammage					= 5.0f;
	public float DammageSpecialAttack1		= 25.0f;
	public float DammageProjectile			= 20.0f;
	public AvatarState DefaultEquipment		= AvatarState.EquipmentNone;
	public float AnimationFrondeAttackSpeed	= 0.5f;
	public GameObject MovementTarget;

	// Enum
	public enum AvatarState
	{
		// Slide
		SlideNone				= 0000000001,
		Slide					= 0000000002,
		// Attack
		AttackNone				= 0000000004,
		FrondeAttack			= 0000000008,
		StandardAttack			= 0000000010,
		SpecialAttack1			= 0000000020,
		// Equipment
		EquipmentNone			= 0000000040,
		EquipmentFronde			= 0000000080,
		EquipmentHammer			= 0000000100,
		// LookAtTargetState
		LookAtTagetFree			= 0000000200,
		LookAtTagetLocked		= 0000000400,
		// Interact
		InteractionNone			= 0000000800,
		InteractionDialogue		= 0000001000,
		InteractionLearnWord	= 0000002000,
	};

	// Public Getters
	public AvatarState CurrentAttackState
	{
		get
		{
			return currentAttackState;
		}
		protected set
		{
			if (StateAllowed(value))
			{
				currentAttackState = value;
			}
			else
			{
				Debug.LogWarning("State '" + value + "' is not allowed for "
					+ transform.root.name + ". Check the Capabilities");
			}
		}
	}
	public AvatarState CurrentSlideState
	{
		get
		{
			return currentSlideState;
		}
		protected set
		{
			if (StateAllowed(value))
			{
				currentSlideState = value;
			}
			else
			{
				Debug.LogWarning("State '" + value + "' is not allowed for "
					+ transform.root.name + ". Check the Capabilities");
			}
		}
	}
	public AvatarState CurrentEquipmentState
	{
		get
		{
			return currentEquipmentState;
		}
		protected set
		{
			if (StateAllowed(value))
			{
				currentEquipmentState = value;
			}
			else
			{
				Debug.LogWarning("State '" + value + "' is not allowed for "
					+ transform.root.name + ". Check the Capabilities");
			}
		}
	}
	public AvatarState CurrentLookAtTargetState
	{
		get
		{
			return currentLookAtTargetState;
		}
		protected set
		{
			if (StateAllowed(value))
			{
				currentLookAtTargetState = value;
			}
			else
			{
				Debug.LogWarning("State '" + value + "' is not allowed for "
					+ transform.root.name + ". Check the Capabilities");
			}
		}
	}
	public AvatarState CurrentInteractionState
	{
		get
		{
			return currentInteractionState;
		}
		protected set
		{
			if (StateAllowed(value))
			{
				currentInteractionState = value;
			}
			else
			{
				Debug.LogWarning("State '" + value + "' is not allowed for "
					+ transform.root.name + ". Check the Capabilities");
			}
		}
	}

	// Protected
	protected AvatarState currentAttackState;
	protected AvatarState currentSlideState;
	protected AvatarState currentEquipmentState;
	protected AvatarState currentLookAtTargetState;
	protected AvatarState currentInteractionState;

	protected AvatarState prevAttackState;
	protected AvatarState prevEquipmentState	= AvatarState.EquipmentNone;
	protected AvatarState prevSlideState;
	protected AvatarState prevLookAtTargetState	= AvatarState.LookAtTagetFree;
	protected AvatarState prevInteractionState	= AvatarState.InteractionNone;

	// Private
	private Vector2 boxColliderAttackSize;
	private Vector3 lookAtTargetRelativePosition = new Vector3(-1.0f, -1.0f, 0.0f);
	private bool CanMove
	{
		get
		{
			return CurrentAttackState != AvatarState.SpecialAttack1 &&
				CurrentAttackState != AvatarState.FrondeAttack &&
				CurrentSlideState == AvatarState.SlideNone &&
				CurrentInteractionState == AvatarState.InteractionNone;
		}
	}
	private bool CanSetLookAtTargetPosition
	{
		get
		{
			return CurrentLookAtTargetState == AvatarState.LookAtTagetLocked ||
			CurrentInteractionState == AvatarState.InteractionNone;
		}
	}

	private new void Awake()
	{
		base.Awake();
		CurrentEquipmentState = AvatarState.EquipmentNone;
		CurrentLookAtTargetState = AvatarState.LookAtTagetFree;
		CurrentInteractionState = AvatarState.InteractionNone;
		CurrentSlideState = AvatarState.SlideNone;
		CurrentAttackState = AvatarState.AttackNone;
		CurrentEquipmentState = DefaultEquipment;

		boxColliderAttackSize = boxCollider2DList[0].size;
		boxCollider2DList[0].size = new Vector2(0.0f, 0.0f);
		LookAtTargetTransform = MovementTarget.transform;
		LookAtTargetTransform.position = new Vector3(transform.position.x - 1.0f,
			transform.position.y - 1.0f,
			transform.position.z);
	}

	protected override void CharacterUpdate()
	{
		if (CanMove)
		{
			MoveAvatar();
		}
		SetMovementValues();

		if (CanSetLookAtTargetPosition)
		{
			SetLookAtTargetPosition();
		}

		MapStateAvatar();
		boxCollider2DList[0].enabled = CurrentMovingState != CharacterState.Dash;

		SetAvatarAnimation();
		SetSpriteDirection();
	}

	// Public Methods

	public void SetLookAtTargetState(AvatarState lookAtTargetState, Vector3? relativePosition = null)
	{
		CurrentLookAtTargetState = lookAtTargetState;
		if (lookAtTargetState == AvatarState.LookAtTagetLocked && relativePosition != null)
		{
			lookAtTargetRelativePosition = (Vector3)relativePosition;
		}
	}

	public void SetEquipmentState(AvatarState equipmentState)
	{
		CurrentEquipmentState = equipmentState;
	}

	public void SetSlideState(AvatarState slideState)
	{
		CurrentSlideState = slideState;
	}
	public void SetInteractionState(AvatarState interactionState)
	{
		CurrentInteractionState = interactionState;
	}

	public void LaunchFrondeProjectile()
	{
		GameObject projectile = Instantiate(GPM.AvatarFrondeProjectile, transform.position, Quaternion.identity);
		projectile.GetComponentInChildren<Projectile>().SetDirection(CurrentOrientation);
	}

	public void EndAttack()
	{
		CurrentAttackState = AvatarState.AttackNone;
		CurrentMovingState = CharacterState.Idle;
		anim.speed = characterAttributes.AnimationSpeed;
		boxCollider2DList[0].size = new Vector2(0.0f, 0.0f);
		SetMovingAnimation(new List<string> { CurrentMovingState.ToString(), CurrentOrientation.ToString() });
		SetSpriteDirection();
	}

	public void EndSlide()
	{
		CurrentSlideState = AvatarState.SlideNone;
		CurrentMovingState = CharacterState.Idle;
	}

	// Private Methods

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.GetComponent<Projectile>() != null &&
			other.gameObject.GetComponent<Projectile>().HitsAvatar)
		{
			ReceiveAttack(1.0f);
			Destroy(other.transform.parent.gameObject);
		}
		if (CurrentFlinchState == CharacterState.Flinch)
		{
			return;
		}
	}
	protected bool StateAllowed(AvatarState state)
	{
		foreach (CharacterAttributes.CharacterCapabilities capability in characterAttributes.Capabilities)
		{
			if (CharacterAttributes.StateAllowed(capability, state))
			{
				return true;
			}
		}
		return false;
	}

	private void MoveAvatar()
	{
		Vector2 targetVelocity = new Vector2(PI.HorizontalInput, PI.VerticalInput);
		//if the vector's length is upper than 1
		if (targetVelocity.magnitude > 1)
		{
			//normalize it <=> set to 1
			targetVelocity.Normalize();
		}

		rigidbodySprite.velocity = targetVelocity * characterAttributes.MaxSpeed;

		SetTargetPosition();
		if (PI.Dash)
		{
			Dash();
		}
	}

	private void SetLookAtTargetPosition()
	{
		LookAtTargetTransform.position = transform.position + lookAtTargetRelativePosition;
	}

	public new void Die()
	{
		SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
	}

	// In the case of the Avatar, the Target is a point that it follows and turn toward.
	//	- When using mouse (TODO): Target is at the location of the click (Diablo Style)
	//	- When using a Gamepad (Default): Target moves around the Avatar, controlled by the joypad
	private void SetTargetPosition()
	{
		//TODO: Handle moving with mouse click (Diablo Style)
		// Using Controller or Keyboard (AWSD)
		if ((PI.HorizontalInput < THRESHOLD_DIRECTION_INPUT * MINUS_1 || PI.HorizontalInput > THRESHOLD_DIRECTION_INPUT) ||
			(PI.VerticalInput < THRESHOLD_DIRECTION_INPUT * MINUS_1 || PI.VerticalInput > THRESHOLD_DIRECTION_INPUT))
		{
			lookAtTargetRelativePosition = new Vector3(PI.HorizontalInput * 3.0f, PI.VerticalInput * 3.0f, 0.0f);

			LookAtTargetTransform.position = transform.position + lookAtTargetRelativePosition;
		}
	}

	private void Dash()
	{
		Vector2 targetVelocity = new Vector2(PI.HorizontalInput, PI.VerticalInput);
		rigidbodySprite.velocity = targetVelocity * characterAttributes.MaxSpeed * characterAttributes.DashSpeed;
	}

	private void MapStateAvatar()
	{
		MapState();
		if (CurrentInteractionState == AvatarState.InteractionNone && IsWeaponEquiped && CurrentAttackState == AvatarState.AttackNone)
		{
			if (PI.Fire2 && CurrentEquipmentState == AvatarState.EquipmentHammer)
			{
				CurrentAttackState = AvatarState.SpecialAttack1;
				CurrentMovingState = CharacterState.AnimationNone;
			}
			else if (PI.Fire1)
			{
				if (CurrentEquipmentState == AvatarState.EquipmentFronde)
				{
					CurrentAttackState = AvatarState.FrondeAttack;
				}
				else if (CurrentEquipmentState == AvatarState.EquipmentHammer)
				{
					CurrentAttackState = AvatarState.StandardAttack;
				}
				CurrentMovingState = CharacterState.AnimationNone;
			}
		}
	}

	private bool IsWeaponEquiped { get { return CurrentEquipmentState == AvatarState.EquipmentFronde ||
												CurrentEquipmentState == AvatarState.EquipmentHammer; } }

	private void SetAvatarAnimation()
	{
		if (CurrentFlinchState != prevFlinchState &&
			CurrentFlinchState != CharacterState.FlinchNone)
		{
			SetFlinchAnimation();
			return;
		}

		if (CurrentSlideState != AvatarState.SlideNone)
		{
			if (CurrentSlideState != prevSlideState)
			{
				SetSlideAnimation();
			}

			return;
		}

		if (CurrentAttackState != prevAttackState &&
			CurrentAttackState != AvatarState.AttackNone)
		{
			SetAttackAnimation();
			boxCollider2DList[0].size = boxColliderAttackSize;
			if (CurrentAttackState == AvatarState.SpecialAttack1 ||
				CurrentAttackState == AvatarState.FrondeAttack)
			{
				return;
			}
		}

		if (CurrentMovingState == CharacterState.AnimationNone)
		{
			return;
		}

		if (CurrentOrientation != prevOrientation ||
			(CurrentMovingState != prevMovingState &&
			CurrentMovingState != CharacterState.AnimationNone))
		{
			SetMovingAnimation(new List<string> { CurrentMovingState.ToString(), CurrentOrientation.ToString() });
			SetSpriteDirection();
			if (CurrentAttackState == AvatarState.FrondeAttack)
			{
				CurrentAttackState = AvatarState.AttackNone;
			}
		}
	}

	private void Attack()
	{
		rigidbodySprite.velocity = Vector3.zero;
		rigidbodySprite.angularVelocity = 0.0f;
	}

	private void SetFlinchAnimation()
	{
		switch (CurrentFlinchState)
		{
			case CharacterState.Flinch:
				anim.SetTrigger(CurrentFlinchState.ToString() + CurrentOrientation.ToString());
				break;
		}
	}

	private void SetSlideAnimation()
	{
		switch (CurrentSlideState)
		{
			case AvatarState.Slide:
				anim.SetTrigger(CurrentSlideState.ToString() + ObjectState.DownLeft);
				break;
		}
	}

	private void SetAttackAnimation()
	{
		switch (CurrentAttackState)
		{
			case AvatarState.FrondeAttack:
				anim.speed = AnimationFrondeAttackSpeed;
				anim.SetTrigger(CurrentAttackState.ToString() + CurrentOrientation.ToString());
				break;
			case AvatarState.StandardAttack:
				anim.SetTrigger(CurrentAttackState.ToString() + CurrentOrientation.ToString());
				break;
			case AvatarState.SpecialAttack1:
				anim.SetTrigger(CurrentAttackState.ToString());
				break;
		}
	}

	protected override void SetLastFrameValues()
	{
		base.SetLastFrameValues();
		prevAttackState = CurrentAttackState;
		prevSlideState = CurrentSlideState;
		prevEquipmentState = CurrentEquipmentState;
		prevLookAtTargetState = CurrentLookAtTargetState;
		prevInteractionState = CurrentInteractionState;
	}
}