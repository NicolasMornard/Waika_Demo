using System.Collections;
using System.Collections.Generic;
using System.Text;

using UnityEngine;

using static ObjectStates;

public class Character : MonoBehaviour
{
	// Public - Inspector
	public CharacterState CurrentStoryState	= CharacterState.StoryNone;

	// Enum
	public enum CharacterState
	{
		// Moving
		AnimationNone	= 0000000001, // When moving animation is suspended for attack or other special moves
		Idle			= 0000000002,
		Walk			= 0000000004,
		Run				= 0000000008,
		Fly				= 0000000010,
		Dash			= 0000000020,

		// Flinch
		FlinchNone		= 0000000040,
		Flinch			= 0000000080,

		// Story Related
		StoryNone		= 0000000100,
		StoryStuck		= 0000000200,

	};

	// Public Getters
	public float CurrentSpeed				{ get; protected set; }
	public Vector3 CurrentDirection			{ get; protected set; }
	public float CurrentDirectionAngle2D	{ get; protected set; }
	public Transform LookAtTargetTransform	{ get; protected set; }
	public bool IsAlive() { return characterAttributes.HP > 0; }

	public float HP
	{
		get
		{
			return characterAttributes.HP;
		}
	}
	public float Mana
	{
		get
		{
			return characterAttributes.Mana;
		}
	}
	public float MaxSpeed
	{
		get
		{
			return characterAttributes.MaxSpeed;
		}
		set
		{
			characterAttributes.MaxSpeed = value;
		}
	}
	public ObjectState CurrentOrientation
	{
		get
		{
			return GamePlayManager.GetOrientationStateForDirectionAngle(CurrentDirectionAngle2D);
		}
	}

	// State
	public CharacterState CurrentMovingState	{
		get
		{
			return currentMovingState;
		}
		protected set
		{
			if (StateAllowed(value))
			{
				currentMovingState = value;
			}
			else
			{
				Debug.LogWarning("State '" + value + "' is not allowed for "
					+ transform.root.name + ". Check the Capabilities");
			}
		}
	}
	public CharacterState CurrentFlinchState
	{
		get
		{
			return currentFlinchState;
		}
		protected set
		{
			if (StateAllowed(value))
			{
				currentFlinchState = value;
			}
			else
			{
				Debug.LogWarning("State '" + value + "' is not allowed for "
					+ transform.root.name + ". Check the Capabilities");
			}
		}
	}

	// Protected
	protected Vector3 prevPos;
	protected CharacterState prevMovingState;
	protected float prevDirectionAngle2D;
	protected ObjectState prevOrientation;
	protected CharacterState prevFlinchState;
	protected CharacterState prevStoryState;
	protected Vector3 prevTargetPosistion;

	protected CharacterAttributes characterAttributes;
	protected Animator anim;
	protected Rigidbody2D rigidbodySprite;
	protected BoxCollider2D[] boxCollider2DList;
	protected SpriteRenderer spriteRenderer;

	// state
	protected CharacterState currentMovingState;
	protected CharacterState currentFlinchState;

	// Private
	private int flinchTimer = 0;

	protected void Awake()
	{
		// CharacterAttributes
		characterAttributes = GetComponent<CharacterAttributes>();
		Debug.Assert(characterAttributes != null, transform.root.name + "Does not have a CharacterAttribute class!");

		CurrentFlinchState = CharacterState.FlinchNone;

		// Animation component
		anim = GetComponent<Animator>();
		anim.enabled = true;
		anim.speed = characterAttributes.AnimationSpeed;

		// Rigidbody2D
		rigidbodySprite = GetComponent<Rigidbody2D>();

		// BoxCollider2D
		boxCollider2DList = GetComponents<BoxCollider2D>();

		// SpriteRenderer
		spriteRenderer = GetComponent<SpriteRenderer>();

		// Character State
		CurrentMovingState = CharacterState.Idle;
		//CurrentOrientation = ObjectState.DownRight;
		SetSpriteDirection();//

		StartCoroutine(ValuesUpdate());
	}

	private void FixedUpdate()
	{
		SetMovementValues();
		MapState();
		HandleFlinch();
	}

	// Public Methods

	public void ReceiveAttack(float dammage = 0.0f)
	{
		CurrentFlinchState = CharacterState.Flinch;
		characterAttributes.HP -= dammage;
	}

	public void EndFlinch()
	{
		CurrentFlinchState = CharacterState.FlinchNone;
		CurrentMovingState = CharacterState.Idle;
		SetSpriteDirection();
	}

	public void SetLookAtTargetTransform(Transform value)
	{
		LookAtTargetTransform = value;
	}

	// Protected Methods
	protected void MapState()
	{
		SetCurrentMovingState();
	}

	protected void SetMovementValues()
	{
		CalculateCurrentDirectionAngle2D();
	}

	protected void CalculateCurrentDirection(Vector3 prevPos)
	{
		if (LookAtTargetTransform != null)
		{
			CurrentDirection = LookAtTargetTransform.position - transform.position;
		}
		else
		{
			CurrentDirection = prevPos - transform.position;
		}
		Debug.Log(CurrentDirection);
	}

	protected void CalculateCurrentDirectionAngle2D()
	{
		CurrentDirectionAngle2D = Vector3.Angle(transform.up, CurrentDirection);
		if (LookAtTargetTransform != null && transform.position.x > LookAtTargetTransform.position.x)
		{
			CurrentDirectionAngle2D *= -1;
		}
		Debug.Log(CurrentDirectionAngle2D);
	}
	protected void SetSpriteDirection()
	{
		if (CurrentStoryState == CharacterState.StoryStuck)
		{
			return;
		}
		spriteRenderer.flipX = CurrentOrientation == ObjectState.UpLeft ||
			CurrentOrientation == ObjectState.Left ||
			CurrentOrientation == ObjectState.DownLeft;
	}

	protected void SetMovingAnimation(List<string> animationNameParts)
	{
		if (animationNameParts == null || animationNameParts.Count == 0)
		{
			return;
		}
		StringBuilder animationNameBuilder = new StringBuilder();
		foreach (string animationNamepart in animationNameParts)
		{
			animationNameBuilder.Append(animationNamepart);
		}

		anim.SetTrigger(animationNameBuilder.ToString());
	}

	protected virtual void SetLastFrameValues()
	{
		prevPos = transform.position;
		prevMovingState = CurrentMovingState;
		prevDirectionAngle2D = CurrentDirectionAngle2D;
		prevOrientation = CurrentOrientation;
		prevFlinchState = CurrentFlinchState;
		prevStoryState = CurrentStoryState;
		if (LookAtTargetTransform != null)
		{
			prevTargetPosistion = LookAtTargetTransform.position;
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

	protected bool StateAllowed(CharacterState state)
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

	// Private Methods
	private void SetCurrentMovingState()
	{
		// We don't execute this if the animation is suspended
		if (CurrentMovingState == CharacterState.AnimationNone)
		{
			return;
		}
		if (CurrentSpeed < characterAttributes.ThresholdIdle)
		{
			CurrentMovingState = CharacterState.Idle;
		}
		else if (CurrentSpeed < characterAttributes.ThresholdRun)
		{
			if (this is Epervier)
			{
				CurrentMovingState = CharacterState.Fly;
			}
			else
			{
				CurrentMovingState = CharacterState.Walk;
			}
		}
		else if (CurrentSpeed < characterAttributes.ThresholdDash)
		{
			if (this is Epervier)
			{
				CurrentMovingState = CharacterState.Fly;
			}
			else
			{
				CurrentMovingState = CharacterState.Run;
			}
		}
		else
		{
			CurrentMovingState = CharacterState.Dash;
		}
	}

	// Private methods
	protected IEnumerator ValuesUpdate()
	{
		while (Application.isPlaying)
		{
			SetLastFrameValues();

			// Wait until the end of the frame
			yield return new WaitForEndOfFrame();

			// Calculate velocity: Velocity = DeltaPosition / DeltaTime
			Vector3 currVel = (prevPos - transform.position) / Time.deltaTime;
			CurrentSpeed = currVel.sqrMagnitude;
			CalculateCurrentDirection(prevPos);
			CalculateCurrentDirectionAngle2D();
			if (CurrentStoryState == CharacterState.StoryStuck)
			{
				if (CurrentStoryState != prevStoryState)
				{
					SetMovingAnimation(new List<string> { CurrentStoryState.ToString(), CurrentOrientation.ToString() });
				}
			}
			else
			{
				CharacterUpdate();
			}
			
		}
	}

	protected virtual void CharacterUpdate()
	{
		if ((CurrentOrientation != prevOrientation ||
					CurrentMovingState != prevMovingState) &&
					CurrentMovingState != CharacterState.AnimationNone)
		{
			Debug.Log(CurrentMovingState.ToString() + " " + CurrentOrientation.ToString());
			SetMovingAnimation(new List<string> { CurrentMovingState.ToString(), CurrentOrientation.ToString() });
		}
		SetSpriteDirection();
	}

		private void HandleFlinch()
	{
		if (CurrentFlinchState == CharacterState.Flinch)
		{
			if (flinchTimer >= characterAttributes.FlinchDuration * (1.0f / Time.deltaTime))
			{
				if (characterAttributes.HP <= 0.0f)
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