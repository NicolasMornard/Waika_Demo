using UnityEngine;

public class Teleport : MonoBehaviour
{
	// Public
	public Transform PositionToTeleportTo;

	// Private
	private Avatar avatar;

	void Start()
	{
		avatar = FindObjectOfType<Avatar>();
	}

	private void Update()
	{
		if (PositionToTeleportTo == null)
		{
			return;
		}

		if (Vector3.Distance(avatar.transform.position, transform.position) <= 2.0f)
		{
			avatar.transform.parent.position = PositionToTeleportTo.position;
			avatar.transform.position = Vector3.zero;
		}
	}
}
