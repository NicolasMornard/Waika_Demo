using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
	private Image progressBarImage;

	private void Awake()
	{
		progressBarImage = GetComponent<Image>();
		if (progressBarImage == null)
		{
			Debug.LogErrorFormat("{0} does not have an Image Component!", name);
		}

		if (progressBarImage.type != Image.Type.Filled)
		{
			Debug.LogErrorFormat("{0} Image is not Filled Type!", name);
		}
	}

	public void SetProgressBarValue(float progressValue)
	{
		if (progressValue < 0.0f)
		{
			progressValue = 0.0f;
		}
		else if (progressValue > 1.0f)
		{
			progressValue = 1.0f;
		}

		progressBarImage.fillAmount = progressValue;
	}
}
