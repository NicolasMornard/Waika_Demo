using System.Collections;
using UnityEngine;
using TMPro;
using TMPro.LearnWord;

public class LearnWordManager : MonoBehaviour
{
    public TextMeshProUGUI TMPBefore;
    public TextMeshProUGUI TMPAfter;
    public GameObject AfterText;
    public TMP_TextSelector TextSelector;
    public Animator TextUpAnimation;

    private bool once;

    private void Awake()
    {
        TMPBefore.enabled = false;
    }
    public void StartLearning(LearnWord learnWord)
    {

        StartCoroutine(LearningProcess(learnWord));
        GameDirector.Avatar.SetInteractionState(Avatar.AvatarState.InteractionLearnWord);
        TMPBefore.SetText(learnWord.Before);
        TMPAfter.SetText(learnWord.After);
        TextSelector.IntializeHoveredList();
    }
    public void EndLearning()
    {
        //TextSelector.HasHoveredEverything = false;
        once = false;
        TextUpAnimation.SetBool("MovingUp", false);
        TMPBefore.enabled = false;
        AfterText.SetActive(false);
        GameDirector.Avatar.SetInteractionState(Avatar.AvatarState.InteractionNone);
        TextSelector.IntializeHoveredList();
    }
    private IEnumerator LearningProcess(LearnWord learnWord)
    {
        TMPBefore.enabled = true;
        yield return new WaitUntil(() => TextSelector.HasHoveredEverything);
        TextUpAnimation.SetBool("MovingUp", true);
        yield return new WaitForSeconds(1);
        AfterText.SetActive(true);
        StartCoroutine(AfterText.GetComponent<TypingScript>().TypingAnimation(true, 0.5f));
        yield return new WaitUntil(() => !AfterText.activeInHierarchy);
        EndLearning();
    }
    void Update()
    {
        if (!once && TMPBefore.enabled && TextSelector.HasHoveredEverything)
        {
            once = true;
        }
    }
}
