using System.Collections.Generic;

using UnityEngine;

public class SlideManager : ElementTrigger
{
	// Public - Inspector
	public List<Transform> Waypoints;
	public float DistanceToReachWaypoint = 0.3f;

	// Public Getters
	public Vector3 CurrentWaypointPosition { get; private set; }

	// Private
	private int currentWaypointIndex = 0;
	private Vector3 startJourneyPosition;
	private float startTime = 0.0f;
	private float journeyLength = 0.0f;

	private void Update()
	{
		if (isTriggered && GameDirector.Avatar.CurrentSlideState == Avatar.AvatarState.Slide)
		{
			HandleWaypoints();
			MoveTowardWaypoint();
		}
	}
	protected override void TriggerAction()
	{
        TriggerSlide();
	}

    protected void TriggerSlide()
    {
        if (Waypoints == null ||
            Waypoints.Count == 0)
        {
            isTriggered = false;
            return;
        }

        if (GameDirector.Avatar.CurrentSlideState != Avatar.AvatarState.Slide)
        {
            isTriggered = true;
            GameDirector.Avatar.SetSlideState(Avatar.AvatarState.Slide);
            currentWaypointIndex = 0;
            CurrentWaypointPosition = Waypoints[currentWaypointIndex].transform.position;
            startTime = Time.time;
            startJourneyPosition = GameDirector.Avatar.transform.position;
            journeyLength = Vector3.Distance(CurrentWaypointPosition, GameDirector.Avatar.transform.position);
            GameDirector.Avatar.SetLookAtTargetState(Avatar.AvatarState.LookAtTagetLocked, new Vector3(-1.0f, -1.0f, 0.0f));
        }
    }

	private void HandleWaypoints()
	{
		if (Waypoints == null ||
			Waypoints.Count == 0)
		{
			isTriggered = false;
			return;
		}

		if (journeyLength <= DistanceToReachWaypoint ||
			Vector3.Distance(CurrentWaypointPosition, GameDirector.Avatar.transform.position) <= DistanceToReachWaypoint)
		{
			if (currentWaypointIndex >= Waypoints.Count)
			{
				isTriggered = false;
				GameDirector.Avatar.EndSlide();
				GameDirector.Avatar.SetLookAtTargetState(Avatar.AvatarState.LookAtTagetFree);
				return;
			}

			CurrentWaypointPosition = Waypoints[currentWaypointIndex].transform.position;
			startTime = Time.time;
			startJourneyPosition = GameDirector.Avatar.transform.position;
			journeyLength = Vector3.Distance(CurrentWaypointPosition, GameDirector.Avatar.transform.position);
			currentWaypointIndex++;
		}
	}

	private void MoveTowardWaypoint()
	{
		if (Waypoints == null ||
			Waypoints.Count == 0)
		{
			return;
		}

		// Distance moved = time * speed.
		float distCovered = (Time.time - startTime) * GameDirector.Avatar.MaxSpeed;

		// Fraction of journey completed = current distance divided by total distance.
		float fracJourney = distCovered / journeyLength;

		// Set our position as a fraction of the distance between the markers.
		GameDirector.Avatar.transform.position = Vector3.Lerp(startJourneyPosition, CurrentWaypointPosition, fracJourney);
	}
}
