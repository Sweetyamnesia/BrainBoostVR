using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TutorialGrabObject : MonoBehaviour
{
    public TutorialManager tutorialManager;
    public int stepIndex = 1; // étape qui valide la saisie

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grab;

    void Start()
    {
        grab = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        grab.selectEntered.AddListener(OnGrab);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        if (tutorialManager.CurrentStepIndex == stepIndex)
            tutorialManager.NextStep();
    }
}
