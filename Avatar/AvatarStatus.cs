using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarStatus : MonoBehaviour
{
	[SerializeField] private float avatarMaxHp = 100.0f;
	[SerializeField] private float avatarMaxMana = 100.0f;

	public float CurrentHp { get; private set; }
	public float CurrentMana { get; private set; }

	private void Awake()
	{
		SetCurrentHpToMax();
		SetCurrentManaToMax();
	}

	public float GetCurrentHpRatio()
	{
		if (avatarMaxHp <= 0.0f)
		{
			Debug.LogErrorFormat("Avatar Max HP must be higher than 0! Current value is: {0}", avatarMaxHp);
			return 0.0f;
		}

		if (CurrentHp <= 0.0f)
		{
			Debug.LogErrorFormat("Avatar Current HP must be higher than or equal to 0! Current value is: {0}", CurrentHp);
			return 0.0f;
		}

		return CurrentHp / avatarMaxHp;
	}

	public float GetCurrentManaRatio()
	{
		if (avatarMaxMana <= 0.0f)
		{
			Debug.LogErrorFormat("Avatar Max HP must be higher than 0! Current value is: {0}", avatarMaxMana);
			return 0.0f;
		}

		if (CurrentMana <= 0.0f)
		{
			Debug.LogErrorFormat("Avatar Current Mana must be higher than or equal to 0! Current value is: {0}", CurrentMana);
			return 0.0f;
		}

		return CurrentMana / avatarMaxMana;
	}

	public void SetCurrentHpToMax()
	{
		CurrentHp = avatarMaxHp;
	}

	public void SetCurrentManaToMax()
	{
		CurrentMana = avatarMaxMana;
	}

	public void SetCurrentHpToZero()
	{
		CurrentHp = 0.0f;
	}

	public void SetCurrentManaToZero()
	{
		CurrentMana = 0.0f;
	}

	public void AddHp(float hpToAdd)
	{
		if (hpToAdd <= 0.0f)
		{
			return;
		}

		CurrentHp += hpToAdd;
		if (CurrentHp > avatarMaxHp)
		{
			CurrentHp = avatarMaxHp;
		}
	}

	public void SubstractHp(float hpToSubstract)
	{
		if (hpToSubstract <= 0.0f)
		{
			return;
		}

		CurrentHp -= hpToSubstract;
		if (CurrentHp <= 0.0f)
		{
			CurrentHp = 0.0f;
		}
	}

	public void AddMana(float manaToAdd)
	{
		if (manaToAdd <= 0.0f)
		{
			return;
		}

		CurrentMana += manaToAdd;
		if (CurrentMana > avatarMaxMana)
		{
			CurrentMana = avatarMaxMana;
		}
	}

	public void SubstractMana(float manaToSubstract)
	{
		if (manaToSubstract <= 0.0f)
		{
			return;
		}

		CurrentMana -= manaToSubstract;
		if (CurrentMana <= 0.0f)
		{
			CurrentMana = 0.0f;
		}
	}
}