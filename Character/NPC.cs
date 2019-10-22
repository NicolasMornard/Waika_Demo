using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class NPC : Character
{
	// Public - Inspector
	public EnemyState DetectionStartingState	= EnemyState.DetectionIdle;
	public EnemyState StartingGeneralState		= EnemyState.Idle;
	public float Dammage						= 5.0f;
	public float DistanceDetectionAvatar		= 10.0f;

	// Enum
	public enum EnemyState
	{
		// General State
		Idle					= 0000000001,
		Search					= 0000000002,
		FollowPath				= 0000000004,
		Fight					= 0000000008,
		Flee					= 0000000010,

		// Waypoint
		WaypointNone			= 0000000020,
		ReachedWaypoint			= 0000000040,
		MovingToWaypoint		= 0000000080,

		// Attack
		AttackNone				= 0000000100,
		StandardAttack			= 0000000200,

		//AvatarDetection
		DetectionIdle			= 0000000400,
		LookingForAvatar		= 0000000800,
		DetectionPullingWay		= 0000001000,
	};

	// Public Getters
	public EnemyState CurrentGeneralState
	{
		get
		{
			return currentGeneralState;
		}
		protected set
		{
			if (StateAllowed(value))
			{
				currentGeneralState = value;
			}
			else
			{
				Debug.LogWarning("State '" + value + "' is not allowed for "
					+ transform.root.name + ". Check the Capabilities");
			}
		}
	}
	public EnemyState CurrentDetectionState
	{
		get
		{
			return currentDetectionState;
		}
		protected set
		{
			if (StateAllowed(value))
			{
				currentDetectionState = value;
			}
			else
			{
				Debug.LogWarning("State '" + value + "' is not allowed for "
					+ transform.root.name + ". Check the Capabilities");
			}
		}
	}
	public EnemyState CurrentAttackState
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
	public EnemyState CurrentWaypointState
	{
		get
		{
			return currentWaypointState;
		}
		protected set
		{
			if (StateAllowed(value))
			{
				currentWaypointState = value;
			}
			else
			{
				Debug.LogWarning("State '" + value + "' is not allowed for "
					+ transform.root.name + ". Check the Capabilities");
			}
		}
	}
	public Vector3 CurrentWaypointPosition
	{
		get
		{
			return currentWaypointPosition;
		}
		protected set
		{
			currentWaypointPosition = value;
		}
	}

	//Protected
	protected EnemyState currentGeneralState;
	protected EnemyState currentAttackState;
	protected EnemyState currentDetectionState;
	protected EnemyState currentWaypointState;
	protected Vector3 currentWaypointPosition;
	protected EnemyState lastFrameAttackState;
	protected EnemyState lastFrameDetectionState;
	protected EnemyState lastFrameWaypointState;

	// Private
	private new void Awake()
	{
		base.Awake();
		CurrentGeneralState		= EnemyState.Idle;
		CurrentAttackState		= EnemyState.AttackNone;
		CurrentDetectionState	= EnemyState.LookingForAvatar;
		CurrentWaypointState	= EnemyState.ReachedWaypoint;
		CurrentDetectionState	= DetectionStartingState;
		CurrentGeneralState		= StartingGeneralState;

		lastFrameAttackState	= EnemyState.AttackNone;
		lastFrameDetectionState	= EnemyState.LookingForAvatar;
		lastFrameWaypointState	= EnemyState.ReachedWaypoint;
	}

	void Start()
	{
		LookAtTargetTransform = GameDirector.Avatar.transform;
	}
	protected override void CharacterUpdate()
	{
		if (CurrentStoryState == CharacterState.StoryStuck)
		{
			if (CurrentStoryState != prevStoryState)
			{
				SetMovingAnimation(new List<string> { CurrentStoryState.ToString(), CurrentOrientation.ToString() });
			}
			return;
		}
		SetMovementValues();
		MapState();

		SetSpriteDirection();
		if (CurrentGeneralState != EnemyState.Flee)
		{
			DetectAvatar();
		}
		else if (CurrentAttackState == EnemyState.StandardAttack &&
			CurrentAttackState != lastFrameAttackState)
		{
			anim.SetTrigger(CurrentAttackState.ToString() + CurrentOrientation.ToString());
		}
		if (CurrentOrientation != prevOrientation ||
			CurrentMovingState != prevMovingState)
		{
			SetMovingAnimation(new List<string> { CurrentMovingState.ToString(), CurrentOrientation.ToString() });
		}
	}

	public void SetGeneralState(EnemyState generalState)
	{
		CurrentGeneralState = generalState;
		switch (CurrentGeneralState)
		{
			case EnemyState.Idle: // TODO
				SetDetectionState(EnemyState.DetectionIdle);
				break;
			case EnemyState.Search: // TODO
				SetDetectionState(EnemyState.LookingForAvatar);
				break;
			case EnemyState.FollowPath: // TODO
				break;
			case EnemyState.Fight: // TODO
				SetDetectionState(EnemyState.LookingForAvatar);
				break;
		}
	}

	public void LaunchProjectile()
	{
		GameObject projectile = Instantiate(GamePlayManager.GPM.EpervierProjectile, transform.position, Quaternion.identity);
		projectile.GetComponentInChildren<Projectile>().SetDirection(CurrentOrientation);
	}

	protected bool StateAllowed(EnemyState state)
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

	private void SetDetectionState(EnemyState detectionState)
	{
		CurrentDetectionState = detectionState;
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (CurrentStoryState == CharacterState.StoryStuck)
		{
			return;
		}

		if (other.gameObject.GetComponent<Avatar>() != null)
		{
			if (CurrentFlinchState != CharacterState.Flinch)
			{
				switch (GameDirector.Avatar.CurrentAttackState)
				{
					case Avatar.AvatarState.StandardAttack:
						ReceiveAttack(GameDirector.Avatar.Dammage);
						break;
					case Avatar.AvatarState.SpecialAttack1:
						ReceiveAttack(GameDirector.Avatar.DammageSpecialAttack1);
						break;
				}
			}
		}
		else if (other.gameObject.GetComponent<Projectile>() != null &&
		other.gameObject.GetComponent<Projectile>().HitsEnemies)
		{
			ReceiveAttack(GameDirector.Avatar.DammageProjectile);
			Destroy(other.transform.parent.gameObject);
			if (this is Epervier && characterAttributes.HP < 160)
			{
				SetGeneralState(EnemyState.Flee);
				CurrentAttackState = EnemyState.AttackNone;

				CurrentMovingState = CharacterState.Walk;
				if ((CurrentOrientation != prevOrientation ||
					CurrentMovingState != prevMovingState))
				{
					SetMovingAnimation(new List<string> { CurrentMovingState.ToString(), CurrentOrientation.ToString() });
				}
			}
		}
		if (CurrentFlinchState == CharacterState.Flinch)
		{
			return;
		}
	}

	private void DetectAvatar()
	{
		if (CurrentDetectionState != EnemyState.LookingForAvatar)
		{
			return;
		}
		if (CurrentAttackState != EnemyState.StandardAttack)
		{
			if (Vector3.Distance(GameDirector.Avatar.transform.position, transform.position) <= DistanceDetectionAvatar)
			{
				CurrentAttackState = EnemyState.StandardAttack;
				CurrentWaypointState = EnemyState.WaypointNone;
				CurrentMovingState = CharacterState.AnimationNone;
				LookAtTargetTransform = GameDirector.Avatar.transform;
			}
		}
		else if (Vector3.Distance(GameDirector.Avatar.transform.position, transform.position) > DistanceDetectionAvatar)
		{
			CurrentAttackState = EnemyState.AttackNone;
			CurrentWaypointState = EnemyState.MovingToWaypoint;
			CurrentMovingState = CharacterState.Walk;
		}
	}

	protected override void SetLastFrameValues()
	{
		base.SetLastFrameValues();
		lastFrameWaypointState = CurrentWaypointState;
		lastFrameAttackState = CurrentAttackState;
	}

	public void StartFading()
	{
		StartCoroutine(FadeIn());
	}

	IEnumerator FadeIn()
	{
		for(float f = 0; f <= 1; f += 0.05f)
		{
			Color c = spriteRenderer.material.color;
			c.a = f;
			spriteRenderer.material.color = c;
			yield return new WaitForSeconds(0.05f);
		}
	}
}
