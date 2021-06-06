using UnityEngine;

public class AvatarMovement : MonoBehaviour
{
	[SerializeField] private float maxSpeed = 10.0f;

	private float maxSpeedSquared;
	private Rigidbody2D localRigidbody;

	private void Awake()
	{
		maxSpeedSquared = maxSpeed * maxSpeed;

		//Init Rigid Body 2D
		localRigidbody = GetComponent<Rigidbody2D>();
		if (localRigidbody == null)
		{
			Debug.LogErrorFormat("{0} does not have a Rigid Body 2D Component!");
		}
	}

	private void Update()
	{
		Move();
	}

	private void Move()
	{
		if (PlayerInput.GetIsMovingInputPressed() &&
			localRigidbody.velocity.SqrMagnitude() < maxSpeedSquared)

		{
			localRigidbody.AddForce(new Vector2(PlayerInput.HorizontalInput, PlayerInput.VerticalInput));
		}
	}
}