using UnityEngine;
using UnityEngine.UI;

//[RequireComponent(typeof(Image))]
public class WeaponUi : MonoBehaviour
{
	private Image equipementImg;
	private Sprite fronde, hammer;

	void Awake()
	{
		fronde = Resources.Load<Sprite>("fronde");
		hammer = Resources.Load<Sprite>("hammer");

		equipementImg = transform.GetComponent<Image>();
	}

	// Update is called once per frame
	void LateUpdate()
	{
		SetEquipmentUISprite();
	}

	private void SetEquipmentUISprite()
	{
		switch (GameDirector.Avatar.CurrentEquipmentState)
		{
			case Avatar.AvatarState.EquipmentFronde:
				equipementImg.sprite = fronde;
				break;
			case Avatar.AvatarState.EquipmentHammer:
				equipementImg.sprite = hammer;
				break;
			case Avatar.AvatarState.EquipmentNone:
			default:
				equipementImg.sprite = null;
				break;
		}
		equipementImg.enabled = equipementImg.sprite != null;
	}
}