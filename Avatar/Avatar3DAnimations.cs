using System.Collections;

using UnityEngine;

public class Avatar3DAnimations : MonoBehaviour
{
	[SerializeField] private Material avatarMaterial;
	[SerializeField] private GameObject avatarSpritePlane;
	[SerializeField] private float timeBetweenFrames = 0.08f;

	private AnimationSprites animationSprites;

	private int spriteIndex = 0;
	private Coroutine updateAnimationCoroutine;

	// Delegates
	public delegate void OnAnimationEndsDelegate();
	public event OnAnimationEndsDelegate OnAnimationEnds;

	public void LaunchAnimation(AnimationSprites animationSprites)
	{
		if (updateAnimationCoroutine != null)
		{
			StopCoroutine(updateAnimationCoroutine);
		}

		if (animationSprites == null || animationSprites.Count == 0)
		{
			// Not launching the animation if we have no Sprite
			return;
		}

		SetAnimationSprites(animationSprites);
		updateAnimationCoroutine = StartCoroutine(UpdateAnimation());
	}

	private void Awake()
	{
		if (avatarMaterial == null)
		{
			Debug.LogErrorFormat("{0} does not have a Material Component!");
		}

		if (avatarSpritePlane == null)
		{
			Debug.LogErrorFormat("{0} does not have a avatarSpritePlane!");
		}
	}

	private void SetAnimationSprites(AnimationSprites newAnimationSprites)
	{
		spriteIndex = 0;
		animationSprites = newAnimationSprites;
	}

	private IEnumerator UpdateAnimation()
	{
		while (true)
		{
			// Not doing anything this frame if there is no animation sprite
			if (animationSprites.Count == 0)
			{
				yield return null;
				continue;
			}

			if (spriteIndex >= animationSprites.Count)
			{
				if (animationSprites.Loop)
				{
					spriteIndex = 0;
				}
				else
				{
					spriteIndex = animationSprites.Count - 1;
					BroadcastAnimationEndedEvent();
				}
			}

			Texture2D newTexture = animationSprites.GetSpriteAtIndex(spriteIndex).texture;
			float textureRatio = (newTexture.height * 1.0f) / (newTexture.width * 1.0f);

			avatarSpritePlane.transform.localScale =
				new Vector3(0.2f, 0.2f * textureRatio, 0.2f * textureRatio);

			avatarMaterial
				.SetTexture("_MainTex", newTexture);

			if (animationSprites.AnimationSpeed <= 0.0f)
			{
				yield return null;
			}
			else
			{
				++spriteIndex;
				yield return new WaitForSeconds(
					timeBetweenFrames / animationSprites.AnimationSpeed);
			}
		}
	}

	private void BroadcastAnimationEndedEvent()
	{
		if (OnAnimationEnds != null)
		{
			OnAnimationEnds();
		}
	}
}