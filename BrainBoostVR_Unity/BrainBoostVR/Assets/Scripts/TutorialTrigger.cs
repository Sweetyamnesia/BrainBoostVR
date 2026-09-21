using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    public TutorialManager tutorialManager;
    public int stepIndex; // L'étape que cette zone valide

    private void OnTriggerEnter(Collider other)
    {
        // Vérifie que c'est le joueur ou l'objet attendu
        if (other.CompareTag("Player") || other.CompareTag("XR Origin") || other.CompareTag("GrabObject"))
        {
            if (tutorialManager.CurrentStepIndex == stepIndex)
            {
                tutorialManager.NextStep();
                gameObject.SetActive(false); // désactive la zone une fois validée
            }
        }
    }
}
