using System.Collections.Generic;

using UnityEngine;

public class AnimationSprites : MonoBehaviour
{
	[SerializeField] private List<Sprite> sprites;
	public float AnimationSpeed = 1.0f;
	public bool Loop = true;

	private void Awake()
	{
		if (Count == 0)
		{
			return;
		}

		foreach (Sprite sprite in sprites)
		{
			if (sprite == null)
			{
				Debug.LogErrorFormat("{0} : one or more sprite is missing!");
			}
		}
	}

	public int Count
	{
		get
		{
			if (sprites == null)
			{
				return 0;
			}

			return sprites.Count;
		}
	}

	public Sprite GetSpriteAtIndex(int index)
	{
		if (index < 0 || index >= Count)
		{
			return null;
		}

		return sprites[index];
	}
}