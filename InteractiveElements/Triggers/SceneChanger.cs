using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : ElementTrigger
{
	// Public
	public string NextScene;

	[SerializeField]
	private bool _useFade = false;

	[ConditionalHide("_useFade", true)]
	[SerializeField]
	private Animator _fadeInStyle;
	[ConditionalHide("_useFade", true)]
	[SerializeField]
	private Animator _fadeOutStyle;
	[ConditionalHide("_useFade", true)]
	[SerializeField]
	private float _fadeDuration = 2.0f;

	void Start()
	{
		if (_useFade)
		{
			Debug.Assert(_fadeInStyle != null, "Fade In Style has not been set!");
			_fadeOutStyle.gameObject.SetActive(true);
			_fadeInStyle.SetBool("IsFadeIn", true);
		}
		Debug.Assert(!string.IsNullOrEmpty(NextScene), "The name of the Next Scene is empty, SceneChanger will not work");
		Debug.Assert(SceneManager.GetSceneByName(NextScene) != null, "The name of the Next Scene" + NextScene + " does not exist in the Build Settings, SceneChanger will not work");
		//GameDirector.GD.SetSceneToLoadAsync(NextScene);
	}
	protected override void TriggerAction()
	{
		StartCoroutine(LoadNextScene());
	}
	IEnumerator LoadNextScene()
	{
		if (_useFade)
		{
			Debug.Assert(_fadeOutStyle != null, "Fade Out Style has not been set!");
			_fadeOutStyle.gameObject.SetActive(true);
			_fadeOutStyle.SetBool("IsFadeOut", true);

			yield return new WaitForSeconds(_fadeDuration);
		}
		else
		{
			yield return new WaitForSeconds(0.0f);
		}

		Debug.Assert(!string.IsNullOrEmpty(NextScene), "The name of the Next Scene is empty, cannot load next scene");
		//GameDirector.GD.GoToLoadedScene();
		GameDirector.GD.LoadScene(NextScene);
	}
}
