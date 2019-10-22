using UnityEngine;

public class ObjectStates : MonoBehaviour
{
	// Enum
	public enum ObjectState
	{
		// Orientation
		Up,
		UpRight,
		UpLeft,
		Down,
		DownLeft,
		DownRight,
		Right,
		Left,
	};

	static public Vector3 GetDirectionFromOrientation(ObjectState orientation)
	{
		switch (orientation)
		{
			case ObjectState.Up:
				return new Vector3(0.0f, 1.0f, 0.0f);
			case ObjectState.UpRight:
				return new Vector3(1.0f, 1.0f, 0.0f);
			case ObjectState.Right:
				return new Vector3(1.0f, 0.0f, 0.0f);
			case ObjectState.DownRight:
				return new Vector3(1.0f, -1.0f, 0.0f);
			case ObjectState.Down:
				return new Vector3(0.0f, -1.0f, 0.0f);
			case ObjectState.DownLeft:
				return new Vector3(-1.0f, -1.0f, 0.0f);
			case ObjectState.Left:
				return new Vector3(-1.0f, 0.0f, 0.0f);
			case ObjectState.UpLeft:
				return new Vector3(-1.0f, 1.0f, 0.0f);
			default:
				return Vector3.zero;
		}
	}
}
