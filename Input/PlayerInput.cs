using UnityEngine;

public static class PlayerInput
{
	public static float HorizontalInput
	{
		get
		{
			return Input.GetAxisRaw("Horizontal");
		}
	}

	public static float VerticalInput
	{
		get
		{
			return Input.GetAxisRaw("Vertical");
		}
	}

	public static float JumpInput
	{
		get
		{
			return Input.GetAxisRaw("Jump");
		}
	}

	public static float Attack1Input
	{
		get
		{
			return Input.GetAxisRaw("Fire1");
		}
	}

	public static MovingInput GetCurrentMovingInput()
	{
		return new MovingInput(GetIsMovingInputPressed(), HorizontalInput, VerticalInput);
	}

	public static bool GetIsMovingInputPressed()
	{
		return HorizontalInput != 0.0f || VerticalInput != 0.0f;
	}

	public static bool GetIsPressingJump()
	{
		return JumpInput > 0.0f;
	}

	public static Vector2 GetMovingInputs()
	{
		return new Vector2(HorizontalInput , VerticalInput);
	}

	public static bool GetIsPressingAttack1()
	{
		return Attack1Input > 0.0f;
	}
}