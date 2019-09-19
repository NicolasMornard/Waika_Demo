using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : ElementTrigger
{
	// Public
	public string NextScene;
	void Start()
	{
		Debug.Assert(!string.IsNullOrEmpty(NextScene), "The name of the Next Scene is empty, SceneChanger will not work");
		Debug.Assert(SceneManager.GetSceneByName(NextScene) != null, "The name of the Next Scene" + NextScene + " does not exist in the Build Settings, SceneChanger will not work");
		GameDirector.GD.SetSceneToLoadAsync(NextScene);
	}
	protected override void TriggerAction()
	{
		Debug.Assert(!string.IsNullOrEmpty(NextScene), "The name of the Next Scene is empty, cannot load next scene");
		GameDirector.GD.GoToLoadedScene();
	}
}
