using System.Collections;

using UnityEngine;

public class AvatarStatusUI : MonoBehaviour
{
	[SerializeField] private ProgressBar hpBar;
	[SerializeField] private ProgressBar manaBar;

	private Coroutine updateBarFillValuesCoroutine;

	private void Awake()
	{
		if (hpBar == null)
		{
			Debug.LogErrorFormat("{0} does not have a Progress Bar set!");
		}

		if (manaBar == null)
		{
			Debug.LogErrorFormat("{0} does not have a Mana Bar set!");
		}
	}

	public void StartUpdatingStatusUI()
	{
		updateBarFillValuesCoroutine = StartCoroutine(UpdateBarFillValues());
	}

	public void StopUpdatingStatusUI()
	{
		if (updateBarFillValuesCoroutine != null)
		{
			StopCoroutine(updateBarFillValuesCoroutine);
		}
	}

	private IEnumerator UpdateBarFillValues()
	{
		while (true)
		{
			UpdateHPBarFillValue();
			UpdateManaBarFillValue();

			//Wait for next frame
			yield return null;
		}
	}

	public void UpdateHPBarFillValue()
	{
		SetHPBarValue(GetHPRatio());
	}

	public void UpdateManaBarFillValue()
	{
		SetManaBarValue(GetManaRatio());
	}

	public float GetHPRatio()
	{
		return GameDirector.GameDirectorInstance.Avatar.GetHPRatio();
	}

	public float GetManaRatio()
	{
		return GameDirector.GameDirectorInstance.Avatar.GetManaRatio();
	}

	public void SetHPBarValue(float hpRatio)
	{
		hpBar.SetProgressBarValue(hpRatio);
	}

	public void SetManaBarValue(float manaRatio)
	{
		manaBar.SetProgressBarValue(manaRatio);
	}
}