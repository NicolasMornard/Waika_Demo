public class EquipmentGiver : ElementTrigger
{
	protected override void TriggerAction()
	{
		GameDirector.Avatar.SetEquipmentState(Avatar.AvatarState.EquipmentHammer);
	}
}
