using UnityEngine;

public class EquipmentGiver : MonoBehaviour
{
	// Private
	private Avatar avatar;

	void Start()
	{
		avatar = FindObjectOfType<Avatar>();
	}

	private void Update()
	{
		if (Vector3.Distance(avatar.transform.position, transform.position) <= 2.3f)
		{
			avatar.SetEquipmentState(Avatar.AvatarState.EquipmentHammer);
			Destroy(gameObject);
		}
	}
}
