using UnityEngine;

public class TargetCamera : MonoBehaviour
{
	private Transform targetTransform;
	private bool followTarget = true;

	public void SetTargetTransform(Transform newTarget)
	{
		targetTransform = newTarget;
	}

	public void MakeCameraFollowTarget(bool followTarget = true)
	{
		this.followTarget = followTarget;
	}

	// Update is called once per frame
	private void LateUpdate()
	{
		if (followTarget)
		{
			FollowTarget();
		}
	}

	private void FollowTarget()
	{
		if (targetTransform == null)
		{
			Debug.LogErrorFormat("{0} does not have a Target to follow!");
			return;
		}

		transform.position = new Vector3(
			targetTransform.position.x,
			 transform.position.y,
			targetTransform.position.z);
	}
}