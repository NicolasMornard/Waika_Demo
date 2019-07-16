using UnityEngine;

public class Character : MonoBehaviour
{
	// Public - Inspector
	public float MaxSpeed			= 10.0f;
	public float DashSpeed			= 50.0f;
	public float AnimationSpeed		= 0.75f;
	public float ThresholdRun		= 0.04f;
	public float ThresholdIdle		= 0.015f;
	public float ThresholdDash		= 0.7f;
	public float ThresholdMoving	= 0.01f;
	public float Threshold			= 10.0f;
	public float DiagonalAngle		= 70.0f;

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

		// Orientation
		Up,
		UpRight,
		UpLeft,
		Down,
		DownLeft,
		DownRight,
		Right,
		Left,
	};

	// Public Getters
	public float CurrentSpeed					{ get; protected set; }
	public Vector3 CurrentDirection				{ get; protected set; }
	public float CurrentDirectionAngle2D		{ get; protected set; }
	public Vector3 LastFramePosition			{ get; protected set; }
	public Transform LookAtTargetTransform		{ get; protected set; }
	public Vector3 LastFrameTargetPosistion		{ get; protected set; }

	// State
	public CharacterState CurrentMovingState	{ get; protected set; }
	public CharacterState LastFrameMovingState	{ get; protected set; }
	public CharacterState CurrentOrientation	{ get; protected set; }
	public CharacterState LastFrameOrientation	{ get; protected set; }
	public CharacterState CurrentFlinchState	{ get; protected set; }
	public CharacterState LastFrameFlinchState	{ get; protected set; }

	// Protected
	protected Animator anim;
	protected Rigidbody2D rigidbodySprite;
	protected BoxCollider2D boxCollider2D;

	// Protected Constants
	protected readonly string VERTICAL_INPUT_NAME	= "Vertical";
	protected readonly string HORIZONTAL_INPUT_NAME	= "Horizontal";

	// Private
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
		boxCollider2D = GetComponent<BoxCollider2D>();

		// Avatar State
		CurrentMovingState = CharacterState.Idle;
		CurrentOrientation = CharacterState.DownLeft;
		SetSpriteDirection();
	}

	void Update()
	{
		SetMovementValues();
		MapState();
		SetSpriteDirection();
		SetLastFramValues();
	}

	void LateUpdate()
	{
		
	}

	// Public Methods

	public void EndFlinch()
	{
		CurrentFlinchState = CharacterState.FlinchNone;
		CurrentMovingState = CharacterState.Idle;
	}

	// Protected Methods

	protected void MapState()
	{
		SetCurrentMovingState();

		// Testing flinch
		if (Input.GetKeyDown("c"))
		{
			CurrentFlinchState = CharacterState.Flinch;
			CurrentMovingState = CharacterState.AnimationNone;
		}

		if (CurrentMovingState == CharacterState.Walk ||
			CurrentMovingState == CharacterState.Run ||
			(LookAtTargetTransform != null && LastFrameTargetPosistion != LookAtTargetTransform.position))
		{
			SetCurrentOrientationState();
		}
	}

	protected void CalculateCurrentSpeed()
	{
		CurrentSpeed = CurrentDirection.magnitude;
	}

	protected void SetMovementValues()
	{
		CalculateCurrentDirection();
		CalculateCurrentDirectionAngle2D();
		CalculateCurrentSpeed();
	}

	protected void CalculateCurrentDirection()
	{
		CurrentDirection = transform.position - LookAtTargetTransform.position;
	}

	protected void CalculateCurrentDirectionAngle2D()
	{
		CurrentDirectionAngle2D = Vector3.Angle(transform.up, CurrentDirection);
		if (transform.position.x > LookAtTargetTransform.position.x)
		{
			CurrentDirectionAngle2D *= -1;
		}
	}

	protected void SetCurrentOrientationState()
	{
		// Moving Up
		if (CurrentDirectionAngle2D >= -10.0f && CurrentDirectionAngle2D < 10.0f)
		{
			CurrentOrientation = CharacterState.Up;
		}
		// Moving Up Right
		else if (CurrentDirectionAngle2D >= 10.0f && CurrentDirectionAngle2D < 80.0f)
		{
			CurrentOrientation = CharacterState.UpRight;
		}
		// Moving Right
		else if (CurrentDirectionAngle2D >= 80.0f && CurrentDirectionAngle2D < 100.0f)
		{
			CurrentOrientation = CharacterState.Right;
		}
		// Moving Down Right
		else if (CurrentDirectionAngle2D >= 100.0f && CurrentDirectionAngle2D < 170.0f)
		{
			CurrentOrientation = CharacterState.DownRight;
		}
		// Moving Down
		else if ((CurrentDirectionAngle2D >= 170.0f && CurrentDirectionAngle2D <= 180.0f) ||
			(CurrentDirectionAngle2D <= -170.0f && CurrentDirectionAngle2D >= -180.0f))
		{
			CurrentOrientation = CharacterState.Down;
		}
		// Moving Down Left
		else if (CurrentDirectionAngle2D <= -100.0f && CurrentDirectionAngle2D > -170.0f)
		{
			CurrentOrientation = CharacterState.DownLeft;
		}
		// Moving Left
		else if (CurrentDirectionAngle2D <= -80.0f && CurrentDirectionAngle2D > -100.0f)
		{
			CurrentOrientation = CharacterState.Left;
		}
		// Moving Up Left
		else if (CurrentDirectionAngle2D <= -10.0f && CurrentDirectionAngle2D > -80.0f)
		{
			CurrentOrientation = CharacterState.UpLeft;
		}
	}

	protected void SetSpriteDirection()
	{
		Vector3 scale = transform.localScale;
		switch (CurrentOrientation)
		{
			case CharacterState.UpLeft:
			case CharacterState.Left:
			case CharacterState.DownLeft:
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

		if (GetComponent<Epervier>() != null)
		{
			Debug.Log(CurrentOrientation);
		}
	}

	protected void SetLastFramValues()
	{
		LastFrameOrientation		= CurrentOrientation;
		LastFrameMovingState		= CurrentMovingState;
		LastFramePosition			= transform.position;
		LastFrameFlinchState		= CurrentFlinchState;
		if (LookAtTargetTransform != null)
		{
			LastFrameTargetPosistion = LookAtTargetTransform.position;
		}
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
}
