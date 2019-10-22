using UnityEngine;

public class Waypoint : MonoBehaviour
{
	// Start is called before the first frame update
	void Awake()
	{
		GetComponentInChildren<MeshRenderer>().enabled = false;
	}
}
