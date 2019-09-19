using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameDirector : MonoBehaviour
{
	public static GameDirector GD;
	public static GameObject soundManager;
	public static Avatar Avatar;
	private static AsyncOperation sceneLoaderAsync;
	private Scene currentScene;
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
		SceneManager.activeSceneChanged += ChangedActiveScene;
	}
	public void SetSceneToLoadAsync(string sceneToLoad)
	{
		StartCoroutine(LoadAsyncScene(sceneToLoad));
	}
	public void GoToLoadedScene()
	{
		// Triggering loaded scene
		sceneLoaderAsync.allowSceneActivation = true;
	}
	private IEnumerator LoadAsyncScene(string sceneToLoad)
	{
		// Loading the next scene in the background so that the change will be faster when triggered
		sceneLoaderAsync = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Single);
		// preventing the next scene to trigger right away
		sceneLoaderAsync.allowSceneActivation = false;

		// Wait until the asynchronous scene fully loads
		while (!sceneLoaderAsync.isDone)
		{
			yield return null;
		}
	}
	private void UnloadAllScenes(int exception = -1)
	{
		int sceneIndex = SceneManager.sceneCountInBuildSettings - 1;
		while (sceneIndex >= 0)
		{
			if (sceneIndex != exception)
			{
				SceneManager.UnloadSceneAsync(sceneIndex);
			}
			sceneIndex--;
		}
	}
	private void ChangedActiveScene(Scene current, Scene next)
	{
		if (currentScene != null && currentScene.name != null)
		{
			SceneManager.UnloadSceneAsync(currentScene);
		}
		currentScene = next;
	}
}
