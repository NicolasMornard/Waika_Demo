using UnityEngine;

public class AvatarPlayer : MonoBehaviour
{
	public enum AvatarOrientation
	{
		None,
		Down,
		Up,
		Right,
		Left,
		DownRight,
		DownLeft,
		UpRight,
		UpLeft,
	}

	public bool CanJump { get; private set; } = true;
	public Avatar3DMovement Avatar3DMovement { get; private set; }
	public Avatar3DAnimations Avatar3DAnimations { get; private set; }
	public TargetCamera AvatarTargetCamera { get; private set; }
	public AvatarStatus AvatarStatus { get; private set; }
	public AvatarStatusUI AvatarStatusUI { get; private set; }
	public AvatarEquipment AvatarEquipment { get; private set; }
	public AvatarEquipmentUI AvatarEquipmentUI { get; private set; }

	public Collider LocalCollider { get; private set; }

	public void SetCanJump(bool newValue)
	{
		CanJump = newValue;
	}

	private void Awake()
	{
		InitComponents();
	}

	private void InitComponents()
	{
		Avatar3DMovement = GetComponent<Avatar3DMovement>();
		if (Avatar3DMovement == null)
		{
			Debug.LogWarningFormat("{0} does not have a Avatar3DMovement Component!", name);
		}

		Avatar3DAnimations = GetComponent<Avatar3DAnimations>();
		if (Avatar3DAnimations == null)
		{
			Debug.LogWarningFormat("{0} does not have a Avatar3DAnimations Component!", name);
		}

		AvatarStatus = GetComponent<AvatarStatus>();
		if (AvatarStatus == null)
		{
			Debug.LogWarningFormat("{0} does not have a AvatarStatus Component!", name);
		}

		AvatarStatusUI = FindObjectOfType<AvatarStatusUI>();
		if (AvatarStatusUI == null)
		{
			Debug.LogWarningFormat("{0} does not have a AvatarStatusUI Component!", name);
		}

		AvatarEquipment = GetComponent<AvatarEquipment>();
		if (AvatarEquipment == null)
		{
			Debug.LogWarningFormat("{0} does not have a AvatarEquipment Component!", name);
		}

		AvatarEquipmentUI = FindObjectOfType<AvatarEquipmentUI>();
		if (AvatarEquipmentUI == null)
		{
			Debug.LogWarningFormat("{0} does not have a AvatarEquipmentUI Component!", name);
		}

		LocalCollider = GetComponentInChildren<Collider>();
		if (LocalCollider == null)
		{
			Debug.LogErrorFormat("{0} does not have a Collider Component!");
		}
	}

	private void InitTargetCamera()
	{
		AvatarTargetCamera = FindObjectOfType<TargetCamera>();
		if (AvatarTargetCamera == null)
		{
			Debug.LogErrorFormat("The scene does not contain a TargetCamera!");
			return;
		}

		AvatarTargetCamera.SetTargetTransform(transform);

		AvatarTargetCamera.MakeCameraFollowTarget();
	}

	public float GetHPRatio()
	{
		return AvatarStatus.GetCurrentHpRatio();
	}

	public float GetManaRatio()
	{
		return AvatarStatus.GetCurrentManaRatio();
	}

	public AvatarEquipment.EquipmentItem GetCurrentAvatarEquipment()
	{
		return AvatarEquipment.GetCurrentEquipmentItem();
	}
}