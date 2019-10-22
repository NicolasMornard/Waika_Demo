using UnityEngine;
using UnityEngine.UI;

public class BarUi : MonoBehaviour
{
	public Type BarType;
	public bool isMainUI;
	public float Min = 0.0f;
	public float Max = 1.0f;
	public enum Type{HP, Mana};

	private Character character;
	private Image barImg;
	private float maxBarValue;

	void Start()
	{
		if (isMainUI)
		{
			character = GameDirector.Avatar;
		}
		else
		{
			character = transform.GetComponentInParent<Character>();
		}
		Debug.Assert(character != null, "The GameObject " + transform.root.name + " has no Character script");

		switch (BarType)
		{
			case Type.HP:
				maxBarValue = character.HP;
				break;
			case Type.Mana:
				maxBarValue = character.Mana;
				break;
		}

		barImg = transform.GetComponent<Image>();
	}
	void Update()
	{
		if (barImg != null)
		{
			float i;
			float adjustedFillAmount;
			float actualBar = 0.0f;
			switch (BarType)
			{
				case Type.HP:
					actualBar = character.HP;
					break;
				case Type.Mana:
					actualBar = character.Mana;
					break;
			}
			i = actualBar / maxBarValue;
			adjustedFillAmount = (i * (Max - Min)) + Min;
			adjustedFillAmount = Mathf.Clamp(adjustedFillAmount, 0, 1);

			barImg.fillAmount = adjustedFillAmount;
		}
	}
}