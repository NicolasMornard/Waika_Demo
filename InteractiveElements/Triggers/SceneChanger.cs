using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : ElementTrigger
{
	// Public
	public string NextScene;

	// Private
	private static AsyncOperation asyncLoad;
	void Awake()
	{
		if (string.IsNullOrEmpty(NextScene))
		{
			Debug.LogWarning("The name of the Next Scene is empty, SceneChanger will not work, disabling");
			return;
		}
		if (SceneManager.GetSceneByName(NextScene) == null)
		{
			Debug.LogWarning("The name of the Next Scene" + NextScene + " does not exist in the Build Settings, SceneChanger will not work, disabling");
			return;
		}
		if (asyncLoad != null)
		{
			asyncLoad.allowSceneActivation = false;
		}
	}
	void Start()
	{
		// Loading the next scene in the background so that the change will be faster when triggered
		asyncLoad = SceneManager.LoadSceneAsync(NextScene);
		// preventing the next scene to trigger right away
		asyncLoad.allowSceneActivation = false;
	}
	protected override void TriggerAction()
	{
		if (string.IsNullOrEmpty(NextScene))
		{
			return;
		}
		// Triggering loaded scene
		asyncLoad.allowSceneActivation = true;
	}
}
