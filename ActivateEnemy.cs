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

	private void Update()
	{
		if (Vector3.Distance(avatar.transform.position, transform.position) <= 3.0f)
		{
			EnemyToActivate.SetDetectionState(Enemy.EnemyState.LookingForAvatar);
			Destroy(gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.GetComponents<Avatar>() == null)
		{
			return;
		}
	}
}
