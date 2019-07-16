using UnityEngine;

public class AvatarCamera : MonoBehaviour
{
	public Transform AvatarTransform;

	// Start is called before the first frame update
	void Start()
	{
		if (AvatarTransform == null)
		{
			AvatarTransform = FindObjectOfType<Avatar>().transform;
		}
	}

	// Update is called once per frame
	void LateUpdate()
	{
		Vector3 cameraPosition = transform.position;
		cameraPosition.x = AvatarTransform.position.x;
		cameraPosition.y = AvatarTransform.position.y;
		transform.position = cameraPosition;
	}
}
