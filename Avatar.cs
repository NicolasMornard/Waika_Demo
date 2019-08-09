using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public class Avatar : Character
{
	// Public - Inspector
	public float Dammage = 5.0f;
	public float DammageSpecialAttack1 = 25.0f;

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
		//Equipment
		EquipmentNone,
		EquipmentHammer,
	};

	// Public Getters
	public AvatarState CurrentAttackState			{ get; private set; }
	public AvatarState LastFrameAttackState			{ get; private set; }
	public AvatarState CurrentSlideState			{ get; private set; }
	public AvatarState LastFrameSlideState			{ get; private set; }
	public AvatarState CurrentEquipmentState		{ get; private set; } = AvatarState.EquipmentNone;
	public AvatarState LastFrameEquipmentState		{ get; private set; } = AvatarState.EquipmentNone;

	// Private
	private float verticalInput;
	private float horizontalInput;
	private Vector2 boxColliderAttackSize;

	// Start is called before the first frame update
	void Start()
	{
		boxColliderAttackSize = boxCollider2DList[1].size;
		boxCollider2DList[1].size = new Vector2(0.0f, 0.0f);
		CurrentAttackState = AvatarState.AttackNone;
		foreach (Transform child in transform.parent)
		{
			// TODO: replace "MovementTarget" with static readonly value
			if (child.name == "MovementTarget")
			{
				LookAtTargetTransform = child.transform;
				LookAtTargetTransform.position = new Vector3(transform.position.x - 1.0f,
					transform.position.y - 1.0f,
					transform.position.z);
				break;
			}
		}
		SetLastFramValuesAvatar();
	}

	// Update is called once per frame
	void Update()
	{
		if (CurrentAttackState != AvatarState.SpecialAttack1 && CurrentSlideState == AvatarState.SlideNone)
		{
			MoveAvatar();
		}
		SetMovementValues();

		MapStateAvatar();
		boxCollider2DList[0].enabled = CurrentMovingState != CharacterState.Dash;

		SetAvatarAnimation();
		SetSpriteDirection();
		SetLastFramValuesAvatar();
		Debug.Log(CurrentSlideState);
	}

	// Public Methods

	public void SetEquipmentState(AvatarState equipmentState)
	{
		CurrentEquipmentState = equipmentState;
	}

	public void SetSlideState(AvatarState slideState)
	{
		CurrentSlideState = slideState;
	}

	public void EndAttack()
	{
		CurrentAttackState = AvatarState.AttackNone;
		CurrentMovingState = CharacterState.Idle;
		boxCollider2DList[1].size = new Vector2(0.0f, 0.0f);
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

		SetTargetPosition();

		Vector2 targetVelocity = new Vector2(horizontalInput, verticalInput);
		rigidbodySprite.velocity = targetVelocity * MaxSpeed;

		// TODO: replace "space" with static Dash input
		if (Input.GetKeyDown("space"))
		{
			Dash();
		}
	}

	public new void Die()
	{
		SceneManager.LoadScene("Level_1", LoadSceneMode.Single);
	}

	// In the case of the Avatar, the Target is a point that it follows and turn toward.
	//	- When using mouse (TODO): Target is at the location of the click (Diablo Style)
	//	- When using a Gamepad (Default): Target moves around the Avatar, controlled by the joypad
	private void SetTargetPosition()
	{
		//TODO: Handle moving with mouse click (Diablo Style)

		// Using Controller or Keyboard (AWSD)
		if (horizontalInput != 0.0f || verticalInput != 0.0f)
		{
			LookAtTargetTransform.position = new Vector3(transform.position.x + horizontalInput,
					transform.position.y + verticalInput,
					transform.position.z);
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
		// TODO: replace "" with static inputs
		if (CurrentEquipmentState == AvatarState.EquipmentHammer)
		{
			if (Input.GetButton("Fire2"))
			{
				CurrentAttackState = AvatarState.SpecialAttack1;
				CurrentMovingState = CharacterState.AnimationNone;
			}
			else if (Input.GetButton("Fire1"))
			{
				CurrentAttackState = AvatarState.StandardAttack;
				CurrentMovingState = CharacterState.AnimationNone;
			}
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

		if (CurrentSlideState != AvatarState.SlideNone)
		{
			if (CurrentSlideState != LastFrameSlideState)
			{
				SetSlideAnimation();
			}

			return;
		}

		if (CurrentAttackState != LastFrameAttackState &&
			CurrentAttackState != AvatarState.AttackNone)
		{
			SetAttackAnimation();
			boxCollider2DList[1].size = boxColliderAttackSize;
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
			CurrentMovingState != LastFrameMovingState &&
			CurrentMovingState != CharacterState.AnimationNone)
		{
			SetMovingAnimation(new List<string> { CurrentMovingState.ToString(), CurrentOrientation.ToString() });
			SetSpriteDirection();
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

	private void SetLastFramValuesAvatar()
	{
		SetLastFramValues();
		LastFrameAttackState = CurrentAttackState;
		LastFrameSlideState = CurrentSlideState;
		LastFrameEquipmentState = CurrentEquipmentState;
	}
}
