using UnityEngine;

using static AvatarPlayer;

public class AvatarStateAttack_1 : AvatarStateBase
{
	protected AvatarStateAttack_1Right avatarStateAttack_1Right;
	protected AvatarStateAttack_1Left avatarStateAttack_1Left;

	protected AvatarStateAttack_1Up avatarStateAttack_1Up;
	protected AvatarStateAttack_1UpRight avatarStateAttack_1UpRight;
	protected AvatarStateAttack_1UpLeft avatarStateAttack_1UpLeft;

	protected AvatarStateAttack_1Down avatarStateAttack_1Down;
	protected AvatarStateAttack_1DownRight avatarStateAttack_1DownRight;
	protected AvatarStateAttack_1DownLeft avatarStateAttack_1DownLeft;

	protected new void Awake()
	{
		base.Awake();

		avatarStateAttack_1Right = GetComponent<AvatarStateAttack_1Right>();
		if (avatarStateAttack_1Right == null)
		{
			Debug.LogWarningFormat("{0} does not have a AvatarStateAttack_1Right Component!");
		}

		avatarStateAttack_1Left = GetComponent<AvatarStateAttack_1Left>();
		if (avatarStateAttack_1Left == null)
		{
			Debug.LogWarningFormat("{0} does not have a AvatarStateAttack_1Left Component!");
		}

		avatarStateAttack_1Up = GetComponent<AvatarStateAttack_1Up>();
		if (avatarStateAttack_1Up == null)
		{
			Debug.LogWarningFormat("{0} does not have a AvatarStateAttack_1Up Component!");
		}

		avatarStateAttack_1UpLeft = GetComponent<AvatarStateAttack_1UpLeft>();
		if (avatarStateAttack_1UpLeft == null)
		{
			Debug.LogWarningFormat("{0} does not have a AvatarStateAttack_1UpLeft Component!");
		}

		avatarStateAttack_1UpRight = GetComponent<AvatarStateAttack_1UpRight>();
		if (avatarStateAttack_1UpRight == null)
		{
			Debug.LogWarningFormat("{0} does not have a AvatarStateAttack_1UpRight Component!");
		}

		avatarStateAttack_1Down = GetComponent<AvatarStateAttack_1Down>();
		if (avatarStateAttack_1Down == null)
		{
			Debug.LogWarningFormat("{0} does not have a AvatarStateAttack_1Down Component!");
		}

		avatarStateAttack_1DownLeft = GetComponent<AvatarStateAttack_1DownLeft>();
		if (avatarStateAttack_1DownLeft == null)
		{
			Debug.LogWarningFormat("{0} does not have a AvatarStateAttack_1DownLeft Component!");
		}

		avatarStateAttack_1DownRight = GetComponent<AvatarStateAttack_1DownRight>();
		if (avatarStateAttack_1DownRight == null)
		{
			Debug.LogWarningFormat("{0} does not have a AvatarStateAttack_1DownRight Component!");
		}
	}

	protected override void EnterState()
	{
		animations.LaunchAnimation(animationSprites);
	}

	protected override void UpdateStateWithRotation(AvatarOrientation orientation)
	{
		switch (orientation)
		{
			case AvatarOrientation.Up:
				SwitchToState(avatarStateAttack_1Up);
				break;
			case AvatarOrientation.Down:
				SwitchToState(avatarStateAttack_1Down);
				break;
			case AvatarOrientation.Right:
				SwitchToState(avatarStateAttack_1Right);
				break;
			case AvatarOrientation.Left:
				SwitchToState(avatarStateAttack_1Left);
				break;
			case AvatarOrientation.DownLeft:
				SwitchToState(avatarStateAttack_1DownLeft);
				break;
			case AvatarOrientation.DownRight:
				SwitchToState(avatarStateAttack_1DownRight);
				break;
			case AvatarOrientation.UpRight:
				SwitchToState(avatarStateAttack_1UpRight);
				break;
			case AvatarOrientation.UpLeft:
				SwitchToState(avatarStateAttack_1UpLeft);
				break;
		}
	}
}