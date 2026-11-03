using System.Collections;
using UnityEngine;
using TMPro;

public class FlashText : MonoBehaviour
{
    public TMP_Text myText;

    private IEnumerator ShowThenHide(string message, float duration)
    {
        myText.text = message;
        myText.enabled = true;
        yield return new WaitForSeconds(duration);
        myText.enabled = false;
    }
    
    void Start()
    {
        StopAllCoroutines();
        StartCoroutine(IntroSequence());
    }

    private IEnumerator IntroSequence()
    {
        yield return ShowThenHide("Welcome to the Game!", 2f);
        yield return new WaitForSeconds(1f);
        yield return ShowThenHide("Press the tap button to shoot a ball.", 3f);
        yield return new WaitForSeconds(1f);
        yield return ShowThenHide("Hold the button to charge up your shot.", 3f);
        yield return new WaitForSeconds(1f);
        yield return ShowThenHide("Shoot balls at the walls to prevent them from closing in!", 3f);
        yield return new WaitForSeconds(1f);
        yield return ShowThenHide("Good luck!", 2f);

    }
}
