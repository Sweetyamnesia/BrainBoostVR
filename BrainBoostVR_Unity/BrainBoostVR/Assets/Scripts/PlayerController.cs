using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.XR.CoreUtils;



public class PlayerController : MonoBehaviour
{

	// Variables for grabbing
	public XRBaseInteractor leftHandInteractor;
	public XRBaseInteractor rightHandInteractor;
	private List<XRGrabInteractable> grabbableObjects;

	// Variables for teleportation
    public TeleportationProvider teleportationProvider;
    public XROrigin xrOrigin;
    private Vector3 startPosition;
    private Vector3 endPosition;

	void Start()
	{
		// Subscription to grab/release events
		grabbableObjects = new List<XRGrabInteractable>(FindObjectsOfType<XRGrabInteractable>());
		foreach (var obj in grabbableObjects)
		{
			obj.selectEntered.AddListener(DetectGrab);
			obj.selectExited.AddListener(DetectRelease);
		}

		// Teleportation event subscription
        if (teleportationProvider != null)
        {
            teleportationProvider.beginLocomotion += OnTeleportStart;
            teleportationProvider.endLocomotion += OnTeleportEnd;
        }
	}

	// Grab detection
	void DetectGrab(SelectEnterEventArgs args)
	{
		EmitEvent("GrabObject");
		Debug.Log(args.interactableObject.transform.name + " saisi");
	}

	// Release detection
	void DetectRelease(SelectExitEventArgs args)
	{
		Debug.Log(args.interactableObject.transform.name + " relâché");
		EmitEvent("ReleaseObject");
	}

	// Start teleportation
    void OnTeleportStart(LocomotionSystem locomotionSystem)
    {
        startPosition = xrOrigin.transform.position;
        Debug.Log("Téléportation commencée depuis " + startPosition);
    }

    // End of teleportation
    void OnTeleportEnd(LocomotionSystem locomotionSystem)
    {
        endPosition = xrOrigin.transform.position;
        Debug.Log("Téléportation terminée à " + endPosition);
        EmitEvent("Teleportation");
    }

	// Event management
	void EmitEvent(string eventName)
	{
		// Send the event name to the ExerciseManager
		Debug.Log("Event émis : " + eventName);
	}

	void DetectDoorOpen()
	{
		// 1. Check if a door has been opened
        // 2. Trigger the event to start the scenario or explanations
        // 3. Add a log for tracking
	}

	void LogInteraction(string logMessage)
	{
		Debug.Log("[INTERACTION] " + logMessage);
	}

	void Update()
	{

	}
	
	
}
