using UnityEngine;
using UnityEngine.UI;

using static AvatarEquipment;

public class AvatarEquipmentUI : MonoBehaviour
{
	[SerializeField] private Sprite slingSprite;
	[SerializeField] private Sprite hammerSprite;

	private Image equipmentImageUI;

	private void Awake()
	{
		equipmentImageUI = GetComponentInChildren<Image>();
		if (equipmentImageUI == null)
		{
			Debug.LogFormat("{0} does not have an Equipment Image set!", name);
		}

		if (slingSprite == null)
		{
			Debug.LogFormat("{0} does not have a Sling Sprite set!", name);
		}

		if (hammerSprite == null)
		{
			Debug.LogFormat("{0} does not have a Hammer Sprite set!", name);
		}
	}

	public void UpdateAvatarEquipmentImage(EquipmentItem newEquipmentItem)
	{
		switch (newEquipmentItem)
		{
			case EquipmentItem.Sling:
				equipmentImageUI.gameObject.SetActive(true);
				equipmentImageUI.sprite = slingSprite;
				break;
			case EquipmentItem.Hammer:
				equipmentImageUI.gameObject.SetActive(true);
				equipmentImageUI.sprite = hammerSprite;
				break;
			default:
				equipmentImageUI.gameObject.SetActive(false);
				break;
		}
	}
}