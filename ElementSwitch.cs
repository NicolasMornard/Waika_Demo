using UnityEngine;

public class ElementSwitch : MonoBehaviour
{
	public GameObject ElementToActivate;
	public GameObject ElementToDeactivate;

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
			ElementToActivate.SetActive(true);
			ElementToDeactivate.SetActive(false);
		}
	}
}
