using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
	public enum CharacterDialog
	{
		Waika,
		WaikaFrere,
	}


	public CharacterDialog[] Names;
	[TextArea(3, 10)]
	public string[] Sentences;
	public List<Sprite> Portraits;
}
