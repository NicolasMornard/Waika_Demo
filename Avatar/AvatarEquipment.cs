using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarEquipment : MonoBehaviour
{
	[SerializeField] private EquipmentItem currentEquipmentItem = EquipmentItem.None;

	public enum EquipmentItem
	{
		None,
		Sling,
		Hammer
	}

	public EquipmentItem GetCurrentEquipmentItem()
	{
		return currentEquipmentItem;
	}

	public void SetCurrentEquipmentItem(EquipmentItem newEquipmentItem)
	{
		if (currentEquipmentItem == newEquipmentItem)
		{
			return;
		}

		currentEquipmentItem = newEquipmentItem;

		//Updating UI with new Equipment
		GameDirector.GameDirectorInstance.Avatar.AvatarEquipmentUI.UpdateAvatarEquipmentImage(currentEquipmentItem);
	}
}
