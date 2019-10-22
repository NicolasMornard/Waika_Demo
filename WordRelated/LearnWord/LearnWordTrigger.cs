//Se script est une copie modifier de "DialogueTrigger.cs" , il faudrais factoriser les deux...
using UnityEngine;

public class LearnWordTrigger : MonoBehaviour
{

    public bool Teach;
    public LearnWord learnWord;
    public bool IsInRange { get; set; }


    public void TriggerLearn()
    {
        FindObjectOfType<LearnWordManager>().StartLearning(learnWord);
    }

    public void TriggerStopLearn()
    {
        FindObjectOfType<LearnWordManager>().EndLearning();
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (Teach && other.gameObject.GetComponent<Avatar>())
        {
            IsInRange = true;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Avatar>())
        {
            TriggerStopLearn();
            IsInRange = false;
        }
    }

    void Update()
    {
        if (IsInRange && PlayerInput.PI.Fire1)
        {
            TriggerLearn();
        }
    }
}
