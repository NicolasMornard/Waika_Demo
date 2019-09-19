using System.Collections.Generic;
using System.Text;

using UnityEngine;

using static ObjectStates;

public class Character : MonoBehaviour
{
	// Public - Inspector
	public float HP							= 10.0f;
	public float MaxSpeed					= 10.0f;
	public float DashSpeed					= 50.0f;
	public float AnimationSpeed				= 0.75f;
	public float FlinchDuration				= 0.5f;
	public float ThresholdRun				= 0.04f;
	public float ThresholdIdle				= 0.015f;
	public float ThresholdDash				= 1.6f;
	public float ThresholdMoving			= 0.01f;
	public float Threshold					= 10.0f;
	public float DiagonalAngle				= 70.0f;
	public CharacterState CurrentStoryState	= CharacterState.StoryNone;

	// Enum
	public enum CharacterState
	{
		// Moving
		AnimationNone, // When moving animation is suspended for attack or other special moves
		Idle,
		Walk,
		Run,
		Dash,

		// Flinch
		FlinchNone,
		Flinch,

		// Story Related
		StoryNone,
		StoryStuck,
	};

	// Public Getters
	public float CurrentSpeed				{ get; protected set; }
	public Vector3 CurrentDirection			{ get; protected set; }
	public float CurrentDirectionAngle2D	{ get; protected set; }
	public Vector3 LastFramePosition		{ get; protected set; }
	public Transform LookAtTargetTransform	{ get; protected set; }
	public Vector3 LastFrameTargetPosistion	{ get; protected set; }

	public ObjectState CurrentOrientation
	{
		get
		{
			return GamePlayManager.GetOrientationStateForDirectionAngle(CurrentDirectionAngle2D);
		}
	}

	// State
	public CharacterState CurrentMovingState	{ get; protected set; }
	public CharacterState LastFrameMovingState	{ get; protected set; }
	public float LastFrameDirectionAngle2D		{ get; protected set; }
	public ObjectState LastFrameOrientation		{ get; protected set; }
	public CharacterState CurrentFlinchState	{ get; protected set; } = CharacterState.FlinchNone;
	public CharacterState LastFrameFlinchState	{ get; protected set; }
	public CharacterState LastFrameStoryState	{ get; protected set; }

	// Protected
	protected Animator anim;
	protected Rigidbody2D rigidbodySprite;
	protected BoxCollider2D[] boxCollider2DList;
	protected SpriteRenderer spriteRenderer;

	// Private
	private int flinchTimer = 0;
	private readonly Vector2 VELOCITY_STOP = new Vector2(0.0f, 0.0f);

	void Awake()
	{
		// Aniamtion component
		anim = GetComponent<Animator>();
		anim.enabled = true;
		anim.speed = AnimationSpeed;

		// Rigidbody2D
		rigidbodySprite = GetComponent<Rigidbody2D>();
		LastFramePosition = transform.position;

		// BoxCollider2D
		boxCollider2DList = GetComponents<BoxCollider2D>();

		// SpriteRenderer
		spriteRenderer = GetComponent<SpriteRenderer>();

		// Character State
		CurrentMovingState = CharacterState.Idle;
		//CurrentOrientation = ObjectState.DownRight;
		SetSpriteDirection();//
	}

	protected void Update()
	{
		if (CurrentStoryState == CharacterState.StoryStuck)
		{
			if (CurrentStoryState != LastFrameStoryState)
			{
				SetMovingAnimation(new List<string> { CurrentStoryState.ToString(), CurrentOrientation.ToString() });
			}
			return;
		}
		SetMovementValues();
		MapState();
		if ((CurrentOrientation != LastFrameOrientation ||
			CurrentMovingState != LastFrameMovingState) &&
			CurrentMovingState != CharacterState.AnimationNone)
		{
			SetMovingAnimation(new List<string> { CurrentMovingState.ToString(), CurrentOrientation.ToString() });
		}
		SetSpriteDirection();
		SetLastFramValues();
	}

	private void FixedUpdate()
	{
		HandleFlinch();
	}

	// Public Methods

	public void ReceiveAttack(float dammage = 0.0f)
	{
		CurrentFlinchState = CharacterState.Flinch;
		HP -= dammage;
	}

	public void EndFlinch()
	{
		CurrentFlinchState = CharacterState.FlinchNone;
		CurrentMovingState = CharacterState.Idle;
		SetSpriteDirection();
	}

	// Protected Methods

	protected void MapState()
	{
		SetCurrentMovingState();
	}

	protected void CalculateCurrentSpeed()
	{
		CurrentSpeed = (transform.position - LastFramePosition).magnitude;
	}

	protected void SetMovementValues()
	{
		CalculateCurrentDirection();
		CalculateCurrentDirectionAngle2D();
		CalculateCurrentSpeed();
	}

	protected void CalculateCurrentDirection()
	{
		if (LookAtTargetTransform != null)
		{
			CurrentDirection = LookAtTargetTransform.position - transform.position;
		}
		else
		{
			CurrentDirection = LastFramePosition - transform.position;
		}
	}

	protected void CalculateCurrentDirectionAngle2D()
	{
		CurrentDirectionAngle2D = Vector3.Angle(transform.up, CurrentDirection);
		if (LookAtTargetTransform != null && transform.position.x > LookAtTargetTransform.position.x)
		{
			CurrentDirectionAngle2D *= -1;
		}
	}

	protected void SetSpriteDirection()
	{
		if (CurrentStoryState == CharacterState.StoryStuck)
		{
			return;
		}

		Vector3 scale = transform.localScale;
		switch (CurrentOrientation)
		{
			case ObjectState.UpLeft:
			case ObjectState.Left:
			case ObjectState.DownLeft:
				if (scale.x > 0)
				{
					scale.x *= -1;
				}
				break;
			default:
				if (scale.x < 0)
				{
					scale.x *= -1;
				}
				break;
		}
		transform.localScale = scale;
	}

	protected void SetMovingAnimation(List<string> animationNameParts)
	{
		if (animationNameParts == null || animationNameParts.Count == 0)
		{
			return;
		}
		StringBuilder animationNameBuilder = new StringBuilder();
		foreach (string animationNameBit in animationNameParts)
		{
			animationNameBuilder.Append(animationNameBit);
		}

		anim.SetTrigger(animationNameBuilder.ToString());
	}

	protected void SetLastFramValues()
	{
		LastFrameDirectionAngle2D	= CurrentDirectionAngle2D;
		LastFrameOrientation		= CurrentOrientation;
		LastFrameMovingState		= CurrentMovingState;
		LastFramePosition			= transform.position;
		LastFrameFlinchState		= CurrentFlinchState;
		LastFrameStoryState			= CurrentStoryState;
		if (LookAtTargetTransform != null)
		{
			LastFrameTargetPosistion = LookAtTargetTransform.position;
		}
	}

	protected void Die()
	{
		if (GetComponent<Avatar>() != null)
		{
			GetComponent<Avatar>().Die();
			return;
		}
		Destroy(gameObject);
	}

	// Private Methods

	private void SetCurrentMovingState()
	{
		// We don't execute this if the animation is suspended
		if (CurrentMovingState == CharacterState.AnimationNone)
		{
			return;
		}
		if (CurrentSpeed < ThresholdIdle)
		{
			CurrentMovingState = CharacterState.Idle;
		}
		else if (CurrentSpeed < ThresholdRun)
		{
			CurrentMovingState = CharacterState.Walk;
		}
		else if (CurrentSpeed < ThresholdDash)
		{
			CurrentMovingState = CharacterState.Run;
		}
		else
		{
			CurrentMovingState = CharacterState.Dash;
		}
	}

	// Private methods

	private void HandleFlinch()
	{
		if (CurrentFlinchState == CharacterState.Flinch)
		{
			if (flinchTimer >= FlinchDuration * (1.0f / Time.deltaTime))
			{
				if (HP <= 0.0f)
				{
					Die();
				}
				else
				{
					CurrentFlinchState = CharacterState.FlinchNone;
					flinchTimer = 0;
					spriteRenderer.color = Color.white;
				}
			}
			else
			{
				flinchTimer++;
				spriteRenderer.color = Color.red;
			}
		}
		else if (flinchTimer > 0 || spriteRenderer.color != Color.white)
		{
			flinchTimer = 0;
			spriteRenderer.color = Color.white;
		}
	}
}
