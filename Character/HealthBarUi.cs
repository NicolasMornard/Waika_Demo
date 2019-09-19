using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUi : MonoBehaviour
{
	private GameObject HealthBar;
	private GameObject ManaBar;
	private Image healthBar;
	private float maxHealth;
	private Image manaBar;
	private float maxMana;

	private const float min = 0.116f;
	private const float max = 0.906f;

	void Start()
	{
		maxHealth = GameDirector.Avatar.HP;
		healthBar = transform.Find("HealthBar").GetComponent<Image>();
		maxMana = GameDirector.Avatar.Mana;
		manaBar = transform.Find("ManaBar").GetComponent<Image>();
	}
	void Update()
	{
		Debug.Assert(maxHealth > 0, "maxHealth must be superior to 0");
		Debug.Assert(maxMana > 0, "maxMana must be superior to 0");
		float i;
		float normalizedFloat;

		healthBar.fillAmount = GameDirector.Avatar.HP / maxHealth;
		i = GameDirector.Avatar.Mana / maxMana;

		//Calculate the normalized float;
		normalizedFloat = (i * (max - min)) + min;

		manaBar.fillAmount = normalizedFloat;
	}
}
