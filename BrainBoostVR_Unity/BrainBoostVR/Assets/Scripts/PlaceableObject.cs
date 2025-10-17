using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(XRGrabInteractable))]
public class PlaceableObject : MonoBehaviour
{
    private XRGrabInteractable grab;
    
    [Header("Hook Detection")]
    public float detectionRadius = 0.2f; // Distance max pour détecter un hook à la release

    private void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();
        grab.selectExited.AddListener(OnRelease);
    }

    private void OnDestroy()
    {
        if (grab != null)
            grab.selectExited.RemoveListener(OnRelease);
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        // Cherche un hook à proximité
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius);
        foreach (var col in colliders)
        {
            HookFinal hook = col.GetComponent<HookFinal>();
            if (hook != null)
            {
                hook.TryPlaceObject(gameObject);
                break; // un seul hook par release
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}