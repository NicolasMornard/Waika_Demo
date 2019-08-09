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

	private void Update()
	{
		if (string.IsNullOrEmpty(NextScene))
		{
			return;
		}

		if (Vector3.Distance(avatar.transform.position, transform.position) <= 5.0f)
		{
			SceneManager.LoadScene(NextScene, LoadSceneMode.Single);
		}
	}
}
