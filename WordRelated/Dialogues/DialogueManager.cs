using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
	/*	public Text NameText;
	public Text DialogueText;*/
	public TextMeshProUGUI NameText;
	public TextMeshProUGUI DialogueText;
	public Image PortraitImage;

	public Animator animator;

	private Queue<string> sentences;
	private Queue<Dialogue.CharacterDialog> names;
	private List<Sprite> imagesToDisplay;
	private IEnumerator typingAnimation;
	private bool isTalking = false;

	// Start is called before the first frame update
	void Awake()
	{
		sentences = new Queue<string>();
		names = new Queue<Dialogue.CharacterDialog>();
	}

	private void Update()
	{
		if (PlayerInput.PI.Fire1 && isTalking)
		{
			DisplayNextSentence();
		}
	}

	public void StartDialogue (Dialogue dialogue)
	{
		isTalking = true;
		GameDirector.Avatar.SetInteractionState(Avatar.AvatarState.InteractionDialogue);
		animator.SetBool("IsOpen", true);

		imagesToDisplay = dialogue.Portraits;

		sentences.Clear();
		names.Clear();

		foreach (string sentence in dialogue.Sentences)
		{
			sentences.Enqueue(sentence);
		}
		foreach (Dialogue.CharacterDialog name in dialogue.Names)
		{
			names.Enqueue(name);
		}
		DisplayNextSentence();

	}
	public void EndDialogue()
	{
		animator.SetBool("IsOpen", false);
		GameDirector.Avatar.SetInteractionState(Avatar.AvatarState.InteractionNone);
		isTalking = false;
	}

	private void DisplayNextSentence()
	{
		if(sentences.Count == 0)
		{
			EndDialogue();
			return;
		}

		string sentence = sentences.Dequeue();
		Dialogue.CharacterDialog name = names.Dequeue();
		PortraitImage.sprite = imagesToDisplay[(int)name];
		if (typingAnimation != null)
		{
			StopCoroutine(typingAnimation);
		}
		typingAnimation = DialogueText.GetComponent<TypingScript>().TypingAnimation(false, 0.05f);
		StartCoroutine(typingAnimation);

		NameText.SetText(name.ToString());
		DialogueText.SetText(sentence.ToString());
	}
}
