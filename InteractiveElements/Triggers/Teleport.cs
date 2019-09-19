using UnityEngine;

public class Teleport : ElementTrigger
{
	// Public
	public Transform PositionToTeleportTo;

	protected override void TriggerAction()
	{
		if (PositionToTeleportTo == null)
		{
			return;
		}
		GameDirector.Avatar.transform.parent.position = PositionToTeleportTo.position;
		GameDirector.Avatar.transform.position = Vector3.zero;
	}
}
