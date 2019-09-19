using UnityEngine;

using static ObjectStates;

public class GamePlayManager : MonoBehaviour
{
	public static GamePlayManager GPM;

	// Projectiles
	public GameObject AvatarFrondeProjectile;
	public GameObject EpervierProjectile;

	public float OrientationVerticalThreshold		= 10.0f;
	public float OrientationHorizontalThreshold		= 10.0f;

	// Public const
	public const float ANGLE_0						= 0.0f;
	public const float ANGLE_90						= 90.0f;
	public const float ANGLE_180					= 180.0f;
	public const int MINUS_1						= -1;
	public const float THRESHOLD_DIRECTION_INPUT	= 0.1f;

	// private static readonly
	private static readonly float[] orientationUpThresholds		= new float[2];
	private static readonly float[] orientationDownThresholds	= new float[2];
	private static readonly float[] orientationRightThresholds	= new float[2];
	private static readonly float[] orientationLeftThresholds	= new float[2];
	void Awake()
	{
		if (GPM != null)
		{
			Destroy(GPM);
		}
		else
		{
			GPM = this;
		}
		DontDestroyOnLoad(this);

		// Setting values
		// Vertical
		orientationUpThresholds[0] = OrientationVerticalThreshold * MINUS_1;
		orientationUpThresholds[1] = OrientationVerticalThreshold;
		orientationDownThresholds[0] = ANGLE_180 - OrientationVerticalThreshold;
		orientationDownThresholds[1] = orientationDownThresholds[0] * MINUS_1;

		//Horizontal
		orientationRightThresholds[0] = ANGLE_90 - OrientationHorizontalThreshold;
		orientationRightThresholds[1] = ANGLE_90 + OrientationHorizontalThreshold;
		orientationLeftThresholds[0] = orientationRightThresholds[1] * MINUS_1;
		orientationLeftThresholds[1] = orientationRightThresholds[0] * MINUS_1;

		// Checks and Warnings
		if (AvatarFrondeProjectile == null)
		{
			Debug.LogWarning("AvatarFrondeProjectile has not been set! Projectiles won't work");
		}
		if (EpervierProjectile == null)
		{
			Debug.LogWarning("EpervierProjectile has not been set! Projectiles won't work");
		}
	}

	public static ObjectState GetOrientationStateForDirectionAngle(float directionAngle)
	{
		// Moving Up
		if (directionAngle >= orientationUpThresholds[0]
			&& directionAngle < orientationUpThresholds[1])
		{
			return ObjectState.Up;
		}
		// Moving Up Right
		else if (directionAngle >= orientationUpThresholds[1] &&
			directionAngle < orientationRightThresholds[0])
		{
			return ObjectState.UpRight;
		}
		// Moving Right
		else if (directionAngle >= orientationRightThresholds[0] &&
			directionAngle < orientationRightThresholds[1])
		{
			return ObjectState.Right;
		}
		// Moving Down Right
		else if (directionAngle >= orientationRightThresholds[1] &&
			directionAngle < orientationDownThresholds[0])
		{
			return ObjectState.DownRight;
		}
		// Moving Down
		else if (directionAngle >= orientationDownThresholds[0] ||
			directionAngle <= orientationDownThresholds[1])
		{
			return ObjectState.Down;
		}
		// Moving Down Left
		else if (directionAngle > orientationDownThresholds[1] &&
			directionAngle <= orientationLeftThresholds[0])
		{
			return ObjectState.DownLeft;
		}
		// Moving Left
		else if (directionAngle > orientationLeftThresholds[0] &&
			directionAngle <= orientationLeftThresholds[1])
		{
			return ObjectState.Left;
		}
		// Moving Up Left
		else
		{
			return ObjectState.UpLeft;
		}
	}
}
