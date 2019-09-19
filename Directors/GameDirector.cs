using UnityEngine;

public class GameDirector : MonoBehaviour
{
	public static GameDirector GD;
	public GameObject soundManager;
	public static Avatar Avatar;
	void Awake()
	{
		if (GD != null)
		{
			Destroy(GD);
		}
		else
		{
			GD = this;
		}
		DontDestroyOnLoad(this);

		Avatar = FindObjectOfType<Avatar>();
	}
}
