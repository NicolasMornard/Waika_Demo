using UnityEngine;
using System.Collections;
using TMPro;

public class TypingScript : MonoBehaviour
{
	//[Range(0, 100)]
	//public int RevealSpeed = 50;

	private TMP_Text m_textMeshPro;

	void Awake()
	{
		// Get Reference to TextMeshPro Component
		m_textMeshPro = GetComponent<TMP_Text>();
		m_textMeshPro.enableWordWrapping = true;
	}

	public IEnumerator TypingAnimation(bool SetOff, float VitesseAffichage)
	{
		// Force an update of the mesh to get valid information.
		m_textMeshPro.ForceMeshUpdate();

		m_textMeshPro.maxVisibleCharacters = 0;
		int counter = 0;

		do
		{
			// How many characters should TextMeshPro display?
			m_textMeshPro.maxVisibleCharacters = counter % (m_textMeshPro.textInfo.characterCount + 1);
			counter++;

			yield return new WaitForSeconds(VitesseAffichage);
		}
		while (m_textMeshPro.maxVisibleCharacters < m_textMeshPro.textInfo.characterCount);

		yield return new WaitForSeconds(2);

		if (SetOff)
		{
			gameObject.SetActive(false);
		}
	}
}