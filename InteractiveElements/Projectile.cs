using UnityEngine;

using static ObjectStates;

public class Projectile : MonoBehaviour
{
	public Vector3 Direction { get; private set; }
	public float AnimationSpeed = 0.75f;
	public float LifeSpan = 3.0f;
		public bool HitsEnemies = true;
		public bool HitsAvatar = false;

		// Public Getters
		public float Speed { get; protected set; } = 30.0f;

	// Protected
	protected Animator anim;
	protected Rigidbody2D rigidbodySprite;
	private SpriteRenderer spriteRenderer;

	// State
	public ObjectState CurrentOrientation { get; protected set; }

	private float timeSinceBirth = 0.0f;

	// Start is called before the first frame update
	void Start()
	{
		// Aniamtion component
		anim = GetComponent<Animator>();
		anim.enabled = true;
		anim.speed = AnimationSpeed;

		// Rigidbody2D
		rigidbodySprite = GetComponent<Rigidbody2D>();

		// SpriteRenderer
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	// Update is called once per frame
	void Update()
	{
		Vector2 targetVelocity = new Vector2(Direction.x * Speed, Direction.y * Speed);
		rigidbodySprite.velocity = targetVelocity;
	}

	void FixedUpdate()
	{
		timeSinceBirth += Time.deltaTime;

		if (timeSinceBirth >= LifeSpan)
		{
			Destroy(transform.parent.gameObject);
		}
	}

	public void SetDirection(ObjectState orientation)
	{
		CurrentOrientation = orientation;
		Direction = GetDirectionFromOrientation(CurrentOrientation);
		spriteRenderer = GetComponent<SpriteRenderer>();
		SetSpriteOrientation();
	}

	private void SetSpriteOrientation()
	{
		switch (CurrentOrientation)
		{
			case ObjectState.Up:
				break;
			case ObjectState.UpRight:
				spriteRenderer.flipX = false;
				spriteRenderer.flipY = true;
				break;
			case ObjectState.Right:
				break;
			case ObjectState.DownRight:
				spriteRenderer.flipX = false;
				spriteRenderer.flipY = false;
				break;
			case ObjectState.Down:
				break;
			case ObjectState.DownLeft:
				spriteRenderer.flipX = true;
				spriteRenderer.flipY = false;
				break;
			case ObjectState.Left:
				break;
			case ObjectState.UpLeft:
				spriteRenderer.flipX = true;
				spriteRenderer.flipY = true;
				break;
		}
	}
}
