using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Locomotion;
using Unity.XR.CoreUtils;

public class PlayerController : MonoBehaviour
{
    // Variables pour la saisie
    public UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor leftHandInteractor;
    public UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor rightHandInteractor;
    private List<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable> grabbableObjects;

    // Variables pour la téléportation
    public UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation.TeleportationProvider teleportationProvider;
    public XROrigin xrOrigin;
    private Vector3 startPosition;
    private Vector3 endPosition;

    void Start()
    {
        // 🔹 Trouver tous les objets XRGrabInteractable dans la scène
        grabbableObjects = new List<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>(
            Object.FindObjectsByType<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>(FindObjectsSortMode.None)
        );

        foreach (var obj in grabbableObjects)
        {
            obj.selectEntered.AddListener(DetectGrab);
            obj.selectExited.AddListener(DetectRelease);
        }

        // 🔹 Abonnement aux événements de téléportation (adapté à la version stable)
        if (teleportationProvider != null)
        {
            teleportationProvider.locomotionStarted += OnTeleportStart;
            teleportationProvider.locomotionEnded += OnTeleportEnd;
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

    // Téléportation (version stable — sans LocomotionEventArgs)
    void OnTeleportStart(LocomotionProvider provider)
    {
        startPosition = xrOrigin.transform.position;
        LogInteraction($"Téléportation commencée depuis {startPosition}");
    }

    void OnTeleportEnd(LocomotionProvider provider)
    {
        endPosition = xrOrigin.transform.position;
        LogInteraction($"Téléportation terminée à {endPosition}");
        EmitEvent("Teleportation");
    }

    // Émission d’événements pour le gestionnaire global
    void EmitEvent(string eventName)
    {
        Debug.Log($"[EVENT] {eventName}");
        // 👉 Tu pourras connecter ton ExerciseManager ou autre système ici
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
