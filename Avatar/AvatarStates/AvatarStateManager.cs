using UnityEngine;

public class AvatarStateManager : MonoBehaviour
{
	public bool CanAttack { get; set; } = true;

	protected AvatarStateIdle avatarStateIdle;

	private void Awake()
	{
		avatarStateIdle = GetComponent<AvatarStateIdle>();
		if (avatarStateIdle == null)
		{
			Debug.LogWarningFormat("{0} does not have a AvatarStateIdle Component!", name);
		}
	}

	private void Start()
	{
		avatarStateIdle.enabled = true;
	}
}