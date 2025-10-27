using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(Collider), typeof(UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable))]
public class VRButtonXR : MonoBehaviour
{
    [Header("Action à exécuter quand le bouton est pressé")]
    public UnityEvent onPress;

    [Header("Visuel survol")]
    public Color highlightColor = Color.yellow;

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable interactable;
    private Renderer rend;
    private Color originalColor;

    private void Awake()
    {
        rend = GetComponent<Renderer>();
        if (rend != null)
            originalColor = rend.material.color;

        interactable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable>();
        interactable.selectEntered.AddListener(OnPressed);
    }

    private void OnDestroy()
    {
        if (interactable != null)
            interactable.selectEntered.RemoveListener(OnPressed);
    }

    private void OnPressed(SelectEnterEventArgs args)
    {
        onPress.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (rend != null)
            rend.material.color = highlightColor;
    }

    private void OnTriggerExit(Collider other)
    {
        if (rend != null)
            rend.material.color = originalColor;
    }
}
