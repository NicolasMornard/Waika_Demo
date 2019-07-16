using UnityEngine;

public class Avatar : Character
{
	// Enum
	public enum AvatarState
	{
		//Slide
		SlideNone,
		Slide,
		// Attack
		AttackNone,
		StandardAttack,
		SpecialAttack1,
	};

	// Public Getters
	public AvatarState CurrentAttackState			{ get; private set; }
	public AvatarState LastFrameAttackState			{ get; private set; }
	public AvatarState CurrentSlideState			{ get; private set; }
	public AvatarState LastFrameSlideState			{ get; private set; }

	// Private
	private float verticalInput;
	private float horizontalInput;

	// Start is called before the first frame update
	void Start()
	{
		CurrentAttackState = AvatarState.AttackNone;
		SetLastFramValuesAvatar();
	}

	// Update is called once per frame
	void Update()
	{
		if (CurrentAttackState != AvatarState.SpecialAttack1)
		{
			MoveAvatar();
		}
		SetMovementValues();

		MapStateAvatar();
		boxCollider2D.enabled = CurrentMovingState != CharacterState.Dash;

		SetAvatarAnimation();
		SetSpriteDirection();
		SetLastFramValuesAvatar();
	}

	private void LateUpdate()
	{
	}

	// Public Methods

	public void EndAttack()
	{
		CurrentAttackState = AvatarState.AttackNone;
		CurrentMovingState = CharacterState.Idle;
	}

	public void EndSlide()
	{
		CurrentSlideState = AvatarState.SlideNone;
		CurrentMovingState = CharacterState.Idle;
	}

	// Private Methods

	private void MoveAvatar()
	{
		verticalInput = Input.GetAxis(VERTICAL_INPUT_NAME);
		horizontalInput = Input.GetAxis(HORIZONTAL_INPUT_NAME);

		Vector2 targetVelocity = new Vector2(horizontalInput, verticalInput);
		rigidbodySprite.velocity = targetVelocity * MaxSpeed;

		if (Input.GetKeyDown("space"))
		{
			Dash();
		}
	}

	private void Dash()
	{
		Vector2 targetVelocity = new Vector2(horizontalInput, verticalInput);
		rigidbodySprite.velocity = targetVelocity * MaxSpeed * DashSpeed;
	}

	private void MapStateAvatar()
	{
		MapState();
		if (Input.GetKeyDown("f"))
		{
			CurrentAttackState = AvatarState.SpecialAttack1;
			CurrentMovingState = CharacterState.AnimationNone;
		}
		else if (Input.GetKeyDown("e"))
		{
			CurrentAttackState = AvatarState.StandardAttack;
			CurrentMovingState = CharacterState.AnimationNone;
		}
		// Testing slide
		else if (Input.GetKeyDown("x"))
		{
			CurrentSlideState = AvatarState.Slide;
			CurrentMovingState = CharacterState.AnimationNone;
		}
	}

	private void SetAvatarAnimation()
	{
		if (CurrentFlinchState != LastFrameFlinchState &&
			CurrentFlinchState != CharacterState.FlinchNone)
		{
			SetFlinchAnimation();
			return;
		}

		if (CurrentSlideState != LastFrameSlideState &&
			CurrentSlideState != AvatarState.SlideNone)
		{
			SetSlideAnimation();
			return;
		}

		if (CurrentAttackState != LastFrameAttackState &&
			CurrentAttackState != AvatarState.AttackNone)
		{
			SetAttackAnimation();
			if (CurrentAttackState == AvatarState.SpecialAttack1)
			{
				return;
			}
		}

		if (CurrentMovingState == CharacterState.AnimationNone)
		{
			return;
		}

		if (CurrentOrientation != LastFrameOrientation ||
			CurrentMovingState != LastFrameMovingState)
		{
			SetMovingAnimation();
			SetSpriteDirection();
		}
	}

	private void Attack()
	{
		rigidbodySprite.velocity = Vector3.zero;
		rigidbodySprite.angularVelocity = 0.0f;
	}

	private void SetMovingAnimation()
	{
		anim.SetTrigger(CurrentMovingState.ToString() + CurrentOrientation.ToString());
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
				anim.SetTrigger(CurrentSlideState.ToString() + CurrentOrientation.ToString());
				break;
		}
	}

	private void SetAttackAnimation()
	{
		switch (CurrentAttackState)
		{
			case AvatarState.StandardAttack:
				anim.SetTrigger(CurrentAttackState.ToString() + CurrentOrientation.ToString());
				break;
			case AvatarState.SpecialAttack1:
				anim.SetTrigger(CurrentAttackState.ToString());
				break;
		}
	}

	protected new void SetMovementValues()
	{
		CalculateCurrentDirection();
		CalculateCurrentDirectionAngle2D();
		CalculateCurrentSpeed();
	}

	private new void CalculateCurrentDirection()
	{
		if (transform.position != LastFramePosition)
		{
			CurrentDirection = transform.position - LastFramePosition;
		}
	}

	private new void CalculateCurrentDirectionAngle2D()
	{
		CurrentDirectionAngle2D = Vector3.Angle(transform.up, CurrentDirection);
		if (rigidbodySprite.velocity.x < 0)
		{
			CurrentDirectionAngle2D *= -1;
		}
	}

	private void SetLastFramValuesAvatar()
	{
		SetLastFramValues();
		LastFrameAttackState = CurrentAttackState;
		LastFrameSlideState = CurrentSlideState;
	}
}
