using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(XRGrabInteractable))]
public class PlaceableObject : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;

    private void Awake()
    {
        // Récupère le composant XRGrabInteractable
        grabInteractable = GetComponent<XRGrabInteractable>();

        // 🔹 Événement déclenché quand l’objet est lâché
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    private void OnDestroy()
    {
        // 🔹 Toujours nettoyer les listeners
        if (grabInteractable != null)
            grabInteractable.selectExited.RemoveListener(OnRelease);
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        // 🔹 Vérifie les colliders proches de la position actuelle de l’objet
        Collider[] colliders = Physics.OverlapSphere(transform.position, 0.15f);
        foreach (var collider in colliders)
        {
            Hook hook = collider.GetComponent<Hook>();
            if (hook != null)
            {
                hook.TryPlaceObject(gameObject);
                break;
            }
        }
    }
}
