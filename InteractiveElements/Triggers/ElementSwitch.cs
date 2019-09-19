using UnityEngine;

public class ElementSwitch : ElementTrigger
{
	public GameObject ElementToActivate;
	public GameObject ElementToDeactivate;

	protected override void TriggerAction()
	{
		ElementToActivate.SetActive(true);
		ElementToDeactivate.SetActive(false);
	}
}
