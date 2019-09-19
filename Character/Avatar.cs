using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

using static GamePlayManager;
using static PlayerInput;

using static ObjectStates;
using System;

public class Avatar : Character
{
	// Public - Inspector
	public float Mana						= 100f;
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
		SlideNone,
		Slide,
		// Attack
		AttackNone,
		FrondeAttack,
		StandardAttack,
		SpecialAttack1,
		// Equipment
		EquipmentNone,
		EquipmentFronde,
		EquipmentHammer,
		// LookAtTargetState
		LookAtAtTagetFree,
		LookAtAtTagetLocked,
	};

	// Public Getters
	public AvatarState CurrentAttackState			{ get; private set; }
	public AvatarState LastFrameAttackState			{ get; private set; }
	public AvatarState CurrentSlideState			{ get; private set; }
	public AvatarState LastFrameSlideState			{ get; private set; }
	public AvatarState CurrentEquipmentState		{ get; private set; } = AvatarState.EquipmentNone;
	public AvatarState LastFrameEquipmentState		{ get; private set; } = AvatarState.EquipmentNone;
	public AvatarState CurrentLookAtTargetState		{ get; private set; } = AvatarState.LookAtAtTagetFree;
	public AvatarState LastFrameLookAtTargetState	{ get; private set; } = AvatarState.LookAtAtTagetFree;

	// Private
	private Vector2 boxColliderAttackSize;
	private Vector3 lookAtTargetRelativePosition = new Vector3(-1.0f, -1.0f, 0.0f);

	// Start is called before the first frame update
	void Start()
	{
		boxColliderAttackSize = boxCollider2DList[1].size;
		boxCollider2DList[1].size = new Vector2(0.0f, 0.0f);
		CurrentAttackState = AvatarState.AttackNone;
		CurrentEquipmentState = DefaultEquipment;
		LookAtTargetTransform = MovementTarget.transform;
		LookAtTargetTransform.position = new Vector3(transform.position.x - 1.0f,
			transform.position.y - 1.0f,
			transform.position.z);
		SetLastFramValuesAvatar();
	}

	// Update is called once per frame
	new void Update()
	{
		if (CurrentAttackState != AvatarState.SpecialAttack1 &&
			CurrentAttackState != AvatarState.FrondeAttack &&
			CurrentSlideState == AvatarState.SlideNone)
		{
			MoveAvatar();
		}
		SetMovementValues();

		if (CurrentLookAtTargetState == AvatarState.LookAtAtTagetLocked)
		{
			SetLookAtTargetPosition();
		}

		MapStateAvatar();
		boxCollider2DList[0].enabled = CurrentMovingState != CharacterState.Dash;

		SetAvatarAnimation();
		SetSpriteDirection();
		SetLastFramValuesAvatar();
	}

	// Public Methods

	public void SetLookAtTargetState(AvatarState lookAtTargetState, Vector3? relativePosition = null)
	{
		CurrentLookAtTargetState = lookAtTargetState;
		if (lookAtTargetState == AvatarState.LookAtAtTagetLocked && relativePosition != null)
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

	public void LaunchFrondeProjectile()
	{
		GameObject projectile = Instantiate(GPM.AvatarFrondeProjectile, transform.position, Quaternion.identity);
		projectile.GetComponentInChildren<Projectile>().SetDirection(CurrentOrientation);
	}

	public void EndAttack()
	{
		CurrentAttackState = AvatarState.AttackNone;
		CurrentMovingState = CharacterState.Idle;
		anim.speed = AnimationSpeed;
		boxCollider2DList[1].size = new Vector2(0.0f, 0.0f);
		//SetMovingAnimation(new List<string> { CurrentMovingState.ToString(), CurrentOrientation.ToString() });
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
	private void MoveAvatar()
	{
		Vector2 targetVelocity = new Vector2(PI.HorizontalInput, PI.VerticalInput);
		rigidbodySprite.velocity = targetVelocity * MaxSpeed;

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
		SceneManager.LoadSceneAsync("Level_1", LoadSceneMode.Single);
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
		rigidbodySprite.velocity = targetVelocity * MaxSpeed * DashSpeed;
	}

	private void MapStateAvatar()
	{
		MapState();
		if (IsWeaponEquiped() && CurrentAttackState == AvatarState.AttackNone)
		{
			if (PI.Fire2 && CurrentEquipmentState == AvatarState.EquipmentHammer)
			{
				CurrentAttackState = AvatarState.SpecialAttack1;
				CurrentMovingState = CharacterState.AnimationNone;
			}
			else if (PI.Fire1)
			{
				if (CurrentEquipmentState == AvatarState.EquipmentFronde &&
					(CurrentOrientation == ObjectState.DownLeft ||
					CurrentOrientation == ObjectState.UpLeft ||
					CurrentOrientation == ObjectState.DownRight ||
					CurrentOrientation == ObjectState.UpRight))
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

	private bool IsWeaponEquiped()
	{
		return CurrentEquipmentState == AvatarState.EquipmentFronde ||
			CurrentEquipmentState == AvatarState.EquipmentHammer;
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

		if (CurrentOrientation != LastFrameOrientation ||
			CurrentMovingState != LastFrameMovingState &&
			CurrentMovingState != CharacterState.AnimationNone)
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
				if (CurrentOrientation == ObjectState.DownLeft ||
					CurrentOrientation == ObjectState.UpLeft ||
					CurrentOrientation == ObjectState.DownRight ||
					CurrentOrientation == ObjectState.UpRight)
				{
					anim.speed = AnimationFrondeAttackSpeed;
					anim.SetTrigger(CurrentAttackState.ToString() + CurrentOrientation.ToString());
				}
				break;
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
		LastFrameLookAtTargetState = CurrentLookAtTargetState;
	}
}
