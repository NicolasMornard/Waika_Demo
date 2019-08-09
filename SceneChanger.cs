using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
	// Public
	public string NextScene;

	// Private
	private Avatar avatar;

	void Start()
	{
		avatar = FindObjectOfType<Avatar>();
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (string.IsNullOrEmpty(NextScene) ||
			other.gameObject.GetComponents<Avatar>() == null)
		{
			return;
		}

		SceneManager.LoadScene(NextScene, LoadSceneMode.Single);
	}
}
