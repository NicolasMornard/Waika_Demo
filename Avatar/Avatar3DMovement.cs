using UnityEngine;

using static AvatarPlayer;

public class Avatar3DMovement : MonoBehaviour
{
	[SerializeField] private float walkingSpeed = 1.0f;
	[SerializeField] private float maxWalkingSpeed = 10.0f;
	[SerializeField] private float jumpForce = 2.0f;
	[SerializeField] private float idleThreashold = 0.5f;

	[SerializeField] private float OrientationCircleRadius = 1.0f;

	private Transform orentationTargetTransform;

	public bool IsMoving
	{
		get
		{
			return localRigidbody.velocity.magnitude > idleThreashold;
		}
	}

	public bool IsGrounded
	{
		get
		{
			return Physics.Raycast(transform.position, -Vector3.up, localCollider.bounds.extents.y + 0.1f);
		}
	}

	public delegate void OnOrientationChangeDelegate(AvatarOrientation newOrientation);
	public event OnOrientationChangeDelegate OnOrientationChange;

	public delegate void OnMovingStatusChangeDelegate(bool isMoving);
	public event OnMovingStatusChangeDelegate OnMovingStatusChange;

	public delegate void OnGroundedStatusChangeDelegate(bool isGrounded);
	public event OnGroundedStatusChangeDelegate OnGroundedStatusChange;

	private float maxSpeedSquared;
	private Rigidbody localRigidbody;
	private Collider localCollider;
	private AvatarOrientation currentOrientation = AvatarOrientation.None;
	private bool bCurrentMovingStatus = false;
	private bool bCurrentGroundedStatus = false;

	public AvatarOrientation GetCurrentAvatarOrientation()
	{
		var orentationTargetDirection = orentationTargetTransform.position - transform.position;

		float orentationAngle = Vector3.Angle(orentationTargetDirection, transform.forward);

		const float angle90 = 90.0f;
		const float angle180 = 180.0f;
		const float angle45 = 45.0f;
		const float diagonalThreashold = 25.0f;

		if (orentationTargetDirection.x > 0.0f)
		{
			if (orentationAngle > angle90 - diagonalThreashold &&
				orentationAngle < angle90 + diagonalThreashold)
			{
				return AvatarOrientation.Right;
			}
		}
		else
		{
			if (orentationAngle > angle90 - diagonalThreashold &&
				orentationAngle < angle90 + diagonalThreashold)
			{
				return AvatarOrientation.Left;
			}
		}

		if (orentationTargetDirection.z > 0.0f)
		{
			if (orentationAngle < diagonalThreashold)
			{
				return AvatarOrientation.Up;
			}

			if (orentationTargetDirection.x > 0.0f)
			{
				return AvatarOrientation.UpRight;
			}

			return AvatarOrientation.UpLeft;
		}

		if (orentationAngle > (angle180 - angle45) + diagonalThreashold)
		{
			return AvatarOrientation.Down;
		}

		if (orentationTargetDirection.x > 0.0f)
		{
			return AvatarOrientation.DownRight;
		}

		return AvatarOrientation.DownLeft;
	}

	public void MoveInDirection(Vector3 direction)
	{
		if (localRigidbody.velocity.sqrMagnitude < maxSpeedSquared)

		{
			float modifiyer = 1.0f;

#if !UNITY_EDITOR
			modifiyer = 10.0f;
#endif

			localRigidbody.AddForce(direction * walkingSpeed * modifiyer);
			SetOrientationTargetPosition(direction);
			DebugGame.DebugGameInstance.DisplayDegug(localRigidbody.velocity.ToString());
		}
	}

	public void Jump()
	{
		float modifiyer = 1.0f;

#if !UNITY_EDITOR
			modifiyer = 10.0f;
#endif

		localRigidbody.AddForce(new Vector3(0.0f, PlayerInput.JumpInput * jumpForce * modifiyer, 0.0f));
	}

	private void Awake()
	{
		InitComponents();
	}

	private void Update()
	{
		if (OnOrientationChange != null)
		{
			var newOrientation = GetCurrentAvatarOrientation();
			if (currentOrientation != newOrientation)
			{
				currentOrientation = newOrientation;
				OnOrientationChange(currentOrientation);
			}
		}

		if (OnMovingStatusChange != null)
		{
			bool bNewMovingStatus = IsMoving;
			if (bCurrentMovingStatus != bNewMovingStatus)
			{
				bCurrentMovingStatus = bNewMovingStatus;
				OnMovingStatusChange(bCurrentMovingStatus);
			}
		}

		if (OnGroundedStatusChange != null)
		{
			bool bNewGroundedStatus = IsGrounded;
			if (bCurrentGroundedStatus != bNewGroundedStatus)
			{
				bCurrentGroundedStatus = bNewGroundedStatus;
				OnGroundedStatusChange(bCurrentGroundedStatus);
			}
		}
	}

	private void InitComponents()
	{
		maxSpeedSquared = maxWalkingSpeed * maxWalkingSpeed;

		localRigidbody = GetComponentInChildren<Rigidbody>();
		if (localRigidbody == null)
		{
			Debug.LogErrorFormat("{0} does not have a Rigidbody Component!");
		}

		localCollider = GetComponentInChildren<Collider>();
		if (localCollider == null)
		{
			Debug.LogErrorFormat("{0} does not have a Collider Component!");
		}

		var orientationTargetComp = GetComponentInChildren<OrientationTarget>();
		if (orientationTargetComp == null)
		{
			Debug.LogErrorFormat("{0} does not have an OrientationTarget Component!");
		}
		else
		{
			orentationTargetTransform = orientationTargetComp.transform;
		}
	}
	private void SetOrientationTargetPosition(Vector3 direction)
	{
		orentationTargetTransform.localPosition = direction.normalized * OrientationCircleRadius;
	}
}