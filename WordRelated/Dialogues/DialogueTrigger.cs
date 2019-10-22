using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
	public bool talks;
	public Dialogue dialogue;

	public bool IsTalking { get; set; }
	public bool IsInRange { get; set; }


	public void TriggerDialogue()
	{
		FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
	}

	public void TriggerStopDialogue()
	{
		FindObjectOfType<DialogueManager>().EndDialogue();
	}

	void OnTriggerStay2D(Collider2D other)
	{
		if (talks && other.gameObject.GetComponent<Avatar>())
		{
			IsInRange = true;
		}
	}
	void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject.GetComponent<Avatar>())
		{
			TriggerStopDialogue();
			IsInRange = false;
			IsTalking = false;
		}
	}

	void Update()
	{
		if (IsInRange && PlayerInput.PI.Fire1 && !IsTalking)
		{
			TriggerDialogue();
			IsTalking = true;
		}
	}
}
