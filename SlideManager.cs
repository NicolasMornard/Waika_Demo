﻿using System.Collections.Generic;

using UnityEngine;

public class SlideManager : MonoBehaviour
{
	// Public - Inspector
	public List<Transform> Waypoints;
	public float DistanceToReachWaypoint = 0.3f;

	// Public Getters
	public Vector3 CurrentWaypointPosition { get; private set; }

	// Private
	private Avatar avatar;
	private int currentWaypointIndex = 0;
	private Vector3 startJourneyPosition;
	private float startTime = 0.0f;
	private float journeyLength = 0.0f;

	void Start()
	{
		avatar = FindObjectOfType<Avatar>();
	}

	private void Update()
	{
		if (avatar.CurrentSlideState == Avatar.AvatarState.Slide)
		{
			HandleWaypoints();
			MoveTowardWaypoint();
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (Waypoints == null ||
			Waypoints.Count == 0)
		{
			return;
		}
		if (other.gameObject.GetComponents<Avatar>() == null)
		{
			return;
		}
		avatar.SetSlideState(Avatar.AvatarState.Slide);
		currentWaypointIndex = 0;
		CurrentWaypointPosition = Waypoints[currentWaypointIndex].transform.position;
		startTime = Time.time;
		startJourneyPosition = avatar.transform.position;
		journeyLength = Vector3.Distance(CurrentWaypointPosition, avatar.transform.position);
	}

	private void HandleWaypoints()
	{
		if (Waypoints == null ||
			Waypoints.Count == 0)
		{
			return;
		}

		if (journeyLength <= DistanceToReachWaypoint ||
			Vector3.Distance(CurrentWaypointPosition, avatar.transform.position) <= DistanceToReachWaypoint)
		{
			if (currentWaypointIndex >= Waypoints.Count)
			{
				avatar.EndSlide();
				return;
			}

			CurrentWaypointPosition = Waypoints[currentWaypointIndex].transform.position;
			startTime = Time.time;
			startJourneyPosition = avatar.transform.position;
			journeyLength = Vector3.Distance(CurrentWaypointPosition, avatar.transform.position);
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
		float speed = avatar.MaxSpeed;

		// Distance moved = time * speed.
		float distCovered = (Time.time - startTime) * speed;

		// Fraction of journey completed = current distance divided by total distance.
		float fracJourney = distCovered / journeyLength;

		// Set our position as a fraction of the distance between the markers.
		avatar.transform.position = Vector3.Lerp(startJourneyPosition, CurrentWaypointPosition, fracJourney);
	}
}