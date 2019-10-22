using UnityEngine;

public class ElementTrigger : MonoBehaviour
{
	public bool DestroyAfterUse = false;

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.GetComponent<Avatar>() == null)
		{
			return;
		}
		TriggerAction();
		if (DestroyAfterUse)
		{
			Destroy(gameObject);
		}
	}
	// Creating for children classes
	protected bool isTriggered = false;
	protected virtual void TriggerAction()
	{
	}
}
