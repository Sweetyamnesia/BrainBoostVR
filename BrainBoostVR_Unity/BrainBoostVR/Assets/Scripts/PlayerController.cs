using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayerController : MonoBehaviour
{
    // Variables pour la saisie
    public XRBaseInteractor leftHandInteractor;
    public XRBaseInteractor rightHandInteractor;
    private List<XRGrabInteractable> grabbableObjects;

    // Variables pour la téléportation
    public TeleportationProvider teleportationProvider;
    public XROrigin xrOrigin;
    private Vector3 startPosition;
    private Vector3 endPosition;

    void Start()
    {
        // Récupérer tous les objets saisissables dans la scène
        grabbableObjects = new List<XRGrabInteractable>(FindObjectsOfType<XRGrabInteractable>());
        foreach (var obj in grabbableObjects)
        {
            obj.selectEntered.AddListener(DetectGrab);
            obj.selectExited.AddListener(DetectRelease);
        }

        // Abonnement aux événements de téléportation
        if (teleportationProvider != null)
        {
            teleportationProvider.beginLocomotion += OnTeleportStart;
            teleportationProvider.endLocomotion += OnTeleportEnd;
        }
    }

    // Gestion de la saisie
    void DetectGrab(SelectEnterEventArgs args)
    {
        EmitEvent("GrabObject");
        LogInteraction($"{args.interactableObject.transform.name} saisi");
    }

    void DetectRelease(SelectExitEventArgs args)
    {
        EmitEvent("ReleaseObject");
        LogInteraction($"{args.interactableObject.transform.name} relâché");
    }

    // Téléportation
    void OnTeleportStart(LocomotionSystem locomotionSystem)
    {
        startPosition = xrOrigin.transform.position;
        LogInteraction($"Téléportation commencée depuis {startPosition}");
    }

    void OnTeleportEnd(LocomotionSystem locomotionSystem)
    {
        endPosition = xrOrigin.transform.position;
        LogInteraction($"Téléportation terminée à {endPosition}");
        EmitEvent("Teleportation");
    }

    // Émission d’événements pour le gestionnaire global
    void EmitEvent(string eventName)
    {
        Debug.Log($"[EVENT] {eventName}");
        // Ici tu peux connecter ton ExerciseManager ou autre système de suivi
    }

    // Logging centralisé
    void LogInteraction(string message)
    {
        Debug.Log($"[INTERACTION] {message}");
    }

    // À implémenter plus tard si nécessaire
    void DetectDoorOpen()
    {
        // Vérifier si une porte ou un tiroir a été ouvert
        // Déclencher des événements si nécessaire
    }
}
