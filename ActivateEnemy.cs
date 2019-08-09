using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateEnemy : MonoBehaviour
{
	public Enemy EnemyToActivate;

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
		EnemyToActivate.SetDetectionState(Enemy.EnemyState.LookingForAvatar);
		gameObject.SetActive(false);
	}
}
