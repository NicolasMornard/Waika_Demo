using UnityEngine;
using UnityEngine.UI;

public class HealthBarUi : MonoBehaviour
{
	private readonly GameObject healthBar;
	private readonly GameObject manaBar;
	private Image healthBarImg;
	private Image manaBarImg;
	private float maxHealth;
	private float maxMana;

	private const float min = 0.116f;
	private const float max = 0.906f;

	void Start()
	{
		Debug.Assert(GameDirector.Avatar != null, "No avatar set in the scene");
		maxHealth = GameDirector.Avatar.HP; //Récupérer à la place la valeur HP du parent (soit avatar, ou enemy)
		Debug.Assert(transform.Find("HealthBar") != null, "Character does not have a healthbar");
		healthBarImg = transform.Find("HealthBar").GetComponent<Image>();
		if (transform.Find("ManaBar") != null)
		{
			maxMana = GameDirector.Avatar.Mana;
			manaBarImg = transform.Find("ManaBar").GetComponent<Image>(); //Si besoin de manaBar, pour ce character. Sinon non
		}
	}
	void Update()
	{
		Debug.Assert(maxHealth > 0, "Max Health must be greater than 0");
		healthBarImg.fillAmount = GameDirector.Avatar.HP / maxHealth;

		if (manaBarImg != null)
		{
			Debug.Assert(maxMana > 0, "Max Mana must be greater than 0");
			float i;
			float adjustedFillAmount;
			i = GameDirector.Avatar.Mana / maxMana;
			adjustedFillAmount = (i * (max - min)) + min;
			adjustedFillAmount = Mathf.Clamp(adjustedFillAmount, 0, 1);

			manaBarImg.fillAmount = adjustedFillAmount;
		}
	}
}
