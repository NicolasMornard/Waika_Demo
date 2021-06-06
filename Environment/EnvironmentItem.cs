using System.Collections;

using UnityEngine;

public class EnvironmentItem : MonoBehaviour
{
	[SerializeField] private float transparencyMinLevel = 0.3f;

	static private readonly string AVATAR_TAG = "Avatar";

	private Material localMaterial;
	private float currentTranparencyLevel = 1.0f;
	private readonly float tranparencyChangeRate = 0.005f;
	private Coroutine updateTranparencyCoroutine;

	private void Awake()
	{
		localMaterial = GetComponentInChildren<MeshRenderer>().materials[0];
		if (localMaterial == null)
		{
			Debug.LogWarningFormat("{0} does not have a Material!");
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag(AVATAR_TAG))
		{
			updateTranparencyCoroutine = StartCoroutine(UpdateTranparency());
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag(AVATAR_TAG))
		{
			if (updateTranparencyCoroutine != null)
			{
				StopCoroutine(updateTranparencyCoroutine);
				StartCoroutine(FadeToMaxTranparencyLevel());
			}
		}
	}

	private IEnumerator UpdateTranparency()
	{
		while (true)
		{
			if (transform.position.z < GameDirector.GameDirectorInstance.Avatar.transform.position.z)
			{
				if (currentTranparencyLevel > transparencyMinLevel)
				{
					FadeTransparencyDown();
				}
			}
			else
			{
				if (currentTranparencyLevel < 1.0f)
				{
					FadeTransparencyUp();
				}
			}

			//Wait for next frame
			yield return null;
		}
	}

	private IEnumerator FadeToMaxTranparencyLevel()
	{
		while (currentTranparencyLevel < 1.0f)
		{
			FadeTransparencyUp();

			//Wait for next frame
			yield return null;
		}
	}

	private void FadeTransparencyDown()
	{
		currentTranparencyLevel -= tranparencyChangeRate;
		if (currentTranparencyLevel < transparencyMinLevel)
		{
			currentTranparencyLevel = transparencyMinLevel;
		}

		SetLocalMaterialTransparency(currentTranparencyLevel);
	}

	private void FadeTransparencyUp()
	{
		currentTranparencyLevel += tranparencyChangeRate;
		if (currentTranparencyLevel > 1.0f)
		{
			currentTranparencyLevel = 1.0f;
		}

		SetLocalMaterialTransparency(currentTranparencyLevel);
	}

	private void SetLocalMaterialTransparency(float transparencyLevel)
	{
		Color newColor = localMaterial.color;
		newColor.a = transparencyLevel;

		localMaterial.color = newColor;
	}
}