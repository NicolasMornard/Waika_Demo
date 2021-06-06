using UnityEngine;
using UnityEngine.UI;

public class DebugGame : MonoBehaviour
{
	[SerializeField] private Text debugTextToDisplay;

	public static DebugGame DebugGameInstance { get; private set; }

	private void Awake()
	{
		if (debugTextToDisplay == null)
		{
			Debug.LogWarningFormat("{0} does not have a Debug Text to Display area");
		}

		if (DebugGameInstance == null)
		{
			DebugGameInstance = this;
		}
	}

	public void DisplayDegug(string text)
	{
		debugTextToDisplay.text = text;
	}
}
