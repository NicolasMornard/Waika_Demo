using System.Collections.Generic;

using UnityEngine;

public class Enemy : Character
{
	// Public - Inspector
	public EnemyState DetectionStartingState	= EnemyState.DetectionIdle;
	public float Dammage						= 5.0f;
	public float DistanceDetectionAvatar		= 2.0f;
	public List<Transform> Waypoints;
	public float DistanceToReachWaypoint		= 0.3f;
	public bool LoopWaypoints					= true;

	// Enum
	public enum EnemyState
	{
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
	public EnemyState CurrentAttackState		{ get; private set; } = EnemyState.AttackNone;
	public EnemyState LastFrameAttackState		{ get; private set; } = EnemyState.AttackNone;
	public EnemyState CurrentDetectionState		{ get; private set; } = EnemyState.LookingForAvatar;
	public EnemyState LastFrameDetectionState	{ get; private set; } = EnemyState.LookingForAvatar;
	public EnemyState CurrentWaypointState		{ get; private set; } = EnemyState.ReachedWaypoint;
	public EnemyState LastFrameWaypointState	{ get; private set; } = EnemyState.ReachedWaypoint;
	public Vector3 CurrentWaypointPosition		{ get; private set; }

	// Private
	private Avatar avatar;
	private int currentWaypointIndex		= 0;
	private Vector3 startJourneyPosition;
	private float startTime					= 0.0f;
	private float journeyLength				= 0.0f;

	void Start()
	{
		avatar = FindObjectOfType<Avatar>();
		LookAtTargetTransform = avatar.transform;
		CurrentDetectionState = DetectionStartingState;
	}

	public void EnemyUpdate()
	{
		if (CurrentDetectionState == EnemyState.DetectionIdle)
		{
			return;
		}
		HandleWaypoints();
		MoveTowardWaypoint();
		//DetectAvatar();
		SetLastFramValuesEnemy();
	}

	public void SetDetectionState(EnemyState detectionState)
	{
		CurrentDetectionState = detectionState;
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.GetComponents<Avatar>() == null)
		{
			return;
		}

		if (CurrentFlinchState != CharacterState.Flinch)
		{
			switch (avatar.CurrentAttackState)
			{
				case Avatar.AvatarState.StandardAttack:
					ReceiveAttack(avatar.Dammage);
					break;
				case Avatar.AvatarState.SpecialAttack1:
					ReceiveAttack(avatar.DammageSpecialAttack1);
					break;
			}
		}

		if (CurrentFlinchState == CharacterState.Flinch)
		{
			return;
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.GetComponents<Avatar>() == null)
		{
			return;
		}

		if (CurrentAttackState == EnemyState.StandardAttack)
		{
			avatar.ReceiveAttack(Dammage);
			CurrentAttackState = EnemyState.AttackNone;
			CurrentDetectionState = EnemyState.DetectionPullingWay;
			startTime = Time.time;
			startJourneyPosition = transform.position;
			journeyLength = Vector3.Distance(avatar.transform.position, transform.position);
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
				CurrentWaypointState = EnemyState.DetectionIdle;
				return;
			}
		}

		LookAtTargetTransform = Waypoints[currentWaypointIndex].transform;
		if (CurrentWaypointState == EnemyState.ReachedWaypoint)
		{
			CurrentWaypointPosition = Waypoints[currentWaypointIndex].transform.position;
			CurrentWaypointState = EnemyState.MovingToWaypoint;
			startTime = Time.time;
			startJourneyPosition = transform.position;
			journeyLength = Vector3.Distance(CurrentWaypointPosition, transform.position);
			currentWaypointIndex++;
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

		if (CurrentDetectionState == EnemyState.DetectionPullingWay)
		{
			if (fracJourney >= 1.0f)
			{
				CurrentWaypointPosition = Waypoints[currentWaypointIndex].transform.position;
				CurrentWaypointState = EnemyState.MovingToWaypoint;
				startTime = Time.time;
				startJourneyPosition = transform.position;
				journeyLength = Vector3.Distance(CurrentWaypointPosition, transform.position);
				CurrentDetectionState = EnemyState.LookingForAvatar;
			}
			else
			{
				fracJourney *= -1.0f;
			}
		}

		// Set our position as a fraction of the distance between the markers.
		transform.position = Vector3.Lerp(startJourneyPosition, CurrentWaypointPosition, fracJourney);
	}

	private void DetectAvatar()
	{
		if (CurrentDetectionState == EnemyState.LookingForAvatar &&
			Vector3.Distance(avatar.transform.position, transform.position) <= DistanceDetectionAvatar)
		{
			CurrentAttackState = EnemyState.StandardAttack;
			CurrentWaypointState = EnemyState.WaypointNone;
			CurrentWaypointPosition = avatar.transform.position;
			startTime = Time.time;
			startJourneyPosition = transform.position;
			journeyLength = Vector3.Distance(CurrentWaypointPosition, transform.position);
		}
	}

	private void SetLastFramValuesEnemy()
	{
		LastFrameWaypointState = CurrentWaypointState;
		LastFrameAttackState = CurrentAttackState;
	}
}
