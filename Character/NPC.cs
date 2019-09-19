using System.Collections.Generic;
using UnityEngine;

public class NPC : Character
{
	// Public - Inspector
	public EnemyState DetectionStartingState	= EnemyState.DetectionIdle;
	public EnemyState StartingGeneralState		= EnemyState.Idle;
	public float Dammage = 5.0f;
	public float DistanceDetectionAvatar		= 10.0f;
	public List<Transform> Waypoints;
	public float DistanceToReachWaypoint		= 0.3f;
	public bool LoopWaypoints = true;

	// Enum
	public enum EnemyState
	{
		// General State
		Idle,
		Search,
		FollowPath,
		Fight,
		Flee,

		// Waypoint
		WaypointNone,
		ReachedWaypoint,
		MovingToWaypoint,

		// Attack
		AttackNone,
		StandardAttack,

		//AvatarDetection
		DetectionIdle,
		LookingForAvatar,
		DetectionPullingWay,
	};

	// Public Getters
	public EnemyState CurrentGeneralState		{ get; private set; }	= EnemyState.Idle;
	public EnemyState CurrentAttackState		{ get; private set; }	= EnemyState.AttackNone;
	public EnemyState LastFrameAttackState		{ get; private set; }	= EnemyState.AttackNone;
	public EnemyState CurrentDetectionState		{ get; private set; }	= EnemyState.LookingForAvatar;
	public EnemyState LastFrameDetectionState	{ get; private set; }	= EnemyState.LookingForAvatar;
	public EnemyState CurrentWaypointState		{ get; private set; }	= EnemyState.ReachedWaypoint;
	public EnemyState LastFrameWaypointState	{ get; private set; }	= EnemyState.ReachedWaypoint;
	public Vector3 CurrentWaypointPosition		{ get; private set; }

	//Protected

	// Private
	private int currentWaypointIndex = 0;
	private Vector3 startJourneyPosition;
	private float startTime = 0.0f;
	private float journeyLength = 0.0f;

	void Start()
	{
		LookAtTargetTransform = GameDirector.Avatar.transform;
		CurrentDetectionState = DetectionStartingState;
		CurrentGeneralState = StartingGeneralState;

		TriggerMove();
	}

	void OnDrawGizmos()
	{
		if (this is Epervier && LookAtTargetTransform != null)
		{
			Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
			Gizmos.DrawLine(transform.position, LookAtTargetTransform.position);
		}
	}

	new protected void Update()
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

		SetSpriteDirection();
		SetLastFramValues();
		if (CurrentGeneralState == EnemyState.Idle)
		{
			return;
		}
		if (CurrentGeneralState != EnemyState.Flee)
		{
			DetectAvatar();
		}
		if (CurrentAttackState == EnemyState.AttackNone)
		{
			HandleWaypoints();
			MoveTowardWaypoint();
		}
		else if (CurrentAttackState == EnemyState.StandardAttack &&
			CurrentAttackState != LastFrameAttackState)
		{
			anim.SetTrigger(CurrentAttackState.ToString() + CurrentOrientation.ToString());
		}
		else if ((CurrentOrientation != LastFrameOrientation ||
			CurrentMovingState != LastFrameMovingState) &&
			CurrentMovingState != CharacterState.AnimationNone)
		{
			SetMovingAnimation(new List<string> { CurrentMovingState.ToString(), CurrentOrientation.ToString() });
		}
		SetLastFramValuesEnemy();
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
			if (this is Epervier && HP < 160)
			{
				SetGeneralState(EnemyState.Flee);
				CurrentAttackState = EnemyState.AttackNone;
				TriggerMove();

				CurrentMovingState = CharacterState.Walk;
				if ((CurrentOrientation != LastFrameOrientation ||
					CurrentMovingState != LastFrameMovingState))
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

	private void TriggerMove()
	{
		if (Waypoints != null && Waypoints.Count > 0)
		{
			Vector3 newCurrentWaypointPosition = Waypoints[currentWaypointIndex].transform.position;
			newCurrentWaypointPosition.z = transform.position.z;
			CurrentWaypointPosition = newCurrentWaypointPosition;
			CurrentWaypointState = EnemyState.MovingToWaypoint;
			startTime = Time.time;
			startJourneyPosition = transform.position;
			journeyLength = Vector3.Distance(CurrentWaypointPosition, transform.position);
		}
	}

	private void HandleWaypoints()
	{
		if (Waypoints == null ||
			Waypoints.Count == 0)
		{
			return;
		}
		if (currentWaypointIndex >= Waypoints.Count)
		{
			if (LoopWaypoints)
			{
				currentWaypointIndex = 0;
			}
			else
			{
				if (this is Epervier)
				{
					SetGeneralState(EnemyState.Search);
					DistanceDetectionAvatar = 2000;
				}
				return;
			}
		}

		LookAtTargetTransform = Waypoints[currentWaypointIndex].transform;
		if (CurrentWaypointState == EnemyState.ReachedWaypoint)
		{
			currentWaypointIndex++;
			TriggerMove();
		}

		if (journeyLength <= DistanceToReachWaypoint ||
			Vector3.Distance(CurrentWaypointPosition, transform.position) <= DistanceToReachWaypoint)
		{
			CurrentWaypointState = EnemyState.ReachedWaypoint;
		}
	}

	private void MoveTowardWaypoint()
	{
		if (Waypoints == null ||
			Waypoints.Count == 0)
		{
			return;
		}
		float speed = MaxSpeed;

		// Distance moved = time * speed.
		float distCovered = (Time.time - startTime) * speed;

		// Fraction of journey completed = current distance divided by total distance.
		float fracJourney = distCovered / journeyLength;

		// Set our position as a fraction of the distance between the markers.
		transform.position = Vector3.Lerp(startJourneyPosition, CurrentWaypointPosition, fracJourney);
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
			if (Waypoints != null && Waypoints.Count > 0)
			{
				LookAtTargetTransform = Waypoints[currentWaypointIndex].transform;
			}
			else
			{
				LookAtTargetTransform = null;
			}
		}
	}

	private void SetLastFramValuesEnemy()
	{
		base.SetLastFramValues();
		LastFrameWaypointState = CurrentWaypointState;
		LastFrameAttackState = CurrentAttackState;
	}
}
