using UnityEngine;

public class EquipmentGiver : MonoBehaviour
{
	// Private
	private Avatar avatar;

	void Start()
	{
		avatar = FindObjectOfType<Avatar>();
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.GetComponents<Avatar>() == null)
		{
			return;
		}
		avatar.SetEquipmentState(Avatar.AvatarState.EquipmentHammer);
		gameObject.SetActive(false);
	}
}
