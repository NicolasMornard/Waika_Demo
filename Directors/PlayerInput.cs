using UnityEngine;

public class PlayerInput : MonoBehaviour
{
	public static PlayerInput PI;

	public float VerticalInput
	{
		get
		{
			return Input.GetAxis(VERTICAL_INPUT_NAME);
		}
		private set
		{
		}
	}
	public float HorizontalInput
	{
		get
		{
			return Input.GetAxis(HORIZONTAL_INPUT_NAME);
		}
		private set
		{
		}
	}

	public bool Fire1
	{
		get
		{
			return Input.GetButtonDown(FIRE1_INPUT_NAME);
		}
		private set
		{
		}
	}
	public bool Fire2
	{
		get
		{
			return Input.GetButtonDown(FIRE2_INPUT_NAME);
		}
		private set
		{
		}
	}

	public bool Dash
	{
		get
		{
			// TODO: replace "space" KeyDown with GetButtonDown
			return Input.GetKeyDown(DASH_INPUT_NAME);
		}
		private set
		{
		}
	}

	// Public Constants
	public const string VERTICAL_INPUT_NAME = "Vertical";
	public const string HORIZONTAL_INPUT_NAME = "Horizontal";
	public const string FIRE1_INPUT_NAME = "Fire1";
	public const string FIRE2_INPUT_NAME = "Fire2";
	public const string DASH_INPUT_NAME = "space";

	void Awake()
	{
		if (PI != null)
		{
			Destroy(PI);
		}
		else
		{
			PI = this;
		}
		DontDestroyOnLoad(this);
	}
}
