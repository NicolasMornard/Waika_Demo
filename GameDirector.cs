using UnityEngine;

public class GameDirector : MonoBehaviour
{
	public static GameDirector GameDirectorInstance { get; private set; }

	public AvatarPlayer Avatar { get; private set; }

	public delegate void MovingInputBroadcastDelegate(MovingInput movingInput);
	public event MovingInputBroadcastDelegate MovingInputBroadcast;

	public delegate void JumpingInputBroadcastDelegate(bool isPressing);
	public event JumpingInputBroadcastDelegate JumpingInputBroadcast;

	public delegate void Attack1InputBroadcastDelegate(bool isPressing);
	public event Attack1InputBroadcastDelegate Attack1InputBroadcast;

	private void Awake()
	{
		if (GameDirectorInstance == null)
		{
			GameDirectorInstance = this;
		}
	}

	private void Start()
	{
		InitAvatar();
		SetHUD();
		StartAvatarProcesses();
	}

	private void Update()
	{
		BoradcastPlayerInput();
	}

	private void InitAvatar()
	{
		Avatar = FindObjectOfType<AvatarPlayer>();
		if (Avatar == null)
		{
			Debug.LogErrorFormat("The scene does not contain an Avatar!");
		}
	}

	private void StartAvatarProcesses()
	{
		Avatar.AvatarStatusUI.StartUpdatingStatusUI();
	}

	private void SetHUD()
	{
		Avatar.AvatarEquipmentUI.UpdateAvatarEquipmentImage(Avatar.GetCurrentAvatarEquipment());
	}

	private void BoradcastPlayerInput()
	{
		// Moving Input
		MovingInputBroadcast?.Invoke(PlayerInput.GetCurrentMovingInput());

		// Jumping Input
		JumpingInputBroadcast?.Invoke(PlayerInput.GetIsPressingJump());

		// Attack1 Input
		Attack1InputBroadcast?.Invoke(PlayerInput.GetIsPressingAttack1());
	}
}