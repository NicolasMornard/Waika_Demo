using UnityEngine;

public class AvatarCamera : MonoBehaviour
{
	public Transform TargetTransform;

	// Start is called before the first frame update
	void Start()
	{
		// Following Avatar if no target set
		if (TargetTransform == null)
		{
			TargetTransform = GameDirector.Avatar.transform;
		}
	}

	void LateUpdate()
	{
		// Following Target
		// TODO: Smooth camera movements
		Vector3 cameraPosition = transform.position;
		cameraPosition.x = TargetTransform.position.x;
		cameraPosition.y = TargetTransform.position.y;
		transform.position = cameraPosition;
	}
}
