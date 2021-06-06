using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	[SerializeField] private GameObject MainMenuButtons;
	[SerializeField] private float delayButtonsActivation = 9.0f;
	[SerializeField] private int newGameSceneIndex = 1;

	private Coroutine displayMenuButtonsAfterDelayCoroutine;

	public void OnNewGameButtonClick()
	{
		StartNewGame();
	}

	public void OnQuitGameButtonClick()
	{
		QuitGame();
	}

	private void Start()
	{
		displayMenuButtonsAfterDelayCoroutine = StartCoroutine(DisplayMenuButtonsAfterDelay(delayButtonsActivation));
	}

	private IEnumerator DisplayMenuButtonsAfterDelay(float delaySeconds)
	{
		yield return new WaitForSeconds(delaySeconds);
		DisplayMenuButtons();
		StopCoroutine(displayMenuButtonsAfterDelayCoroutine);
	}

	private void DisplayMenuButtons()
	{
		MainMenuButtons.SetActive(true);
	}

	private void StartNewGame()
	{
		SceneManager.LoadScene(newGameSceneIndex);
	}

	private void QuitGame()
	{
		Application.Quit();
	}
}