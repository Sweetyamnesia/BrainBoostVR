using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


public class PlayerController : MonoBehaviour
{
	// Variables for logging
	public XRBaseInteractor leftHandInteractor;
	public XRBaseInteractor rightHandInteractor;
	private List<XRGrabInteractable> grabbableObjects;

	// Start is called before the first frame update
	void Start()
	{
		// Search for all XR Grab Interactable objects in the scene
		grabbableObjects = new List<XRGrabInteractable>(FindObjectsOfType<XRGrabInteractable>());

		// For each object, subscribe to DetectGrab and DetectRelease
		foreach (var obj in grabbableObjects)
		{
			obj.selectEntered.AddListener(DetectGrab);
			obj.selectExited.AddListener(DetectRelease);
		}
	}

	void DetectGrab(SelectEnterEventArgs args)
	{
		EmitEvent("GrabObject");
		Debug.Log(args.interactableObject.transform.name + " saisi");
	}

	void DetectRelease(SelectExitEventArgs args)
	{
		Debug.Log(args.interactableObject.transform.name + " relâché");
		EmitEvent("ReleaseObject");
	}

	void EmitEvent(string eventName)
	{
		// Send the event name to the ExerciseManager
		Debug.Log("Event émis : " + eventName);
	}

	void DetectTeleport()
	{
		// 1. Check if the player is moving or teleporting
        // 2. Add a log: position + timestamp
        // 3. Check that teleportation does not go through walls
	}

	void DetectDoorOpen()
	{
		// 1. Check if a door has been opened
        // 2. Trigger the event to start the scenario or explanations
        // 3. Add a log for tracking
	}

	void LogInteraction()
	{
		 // 1. Add a message to the console/log for debugging
        // 2. Example: “The player picked up item X at 12:34”
	}

	// Update is called once per frame
	void Update()
	{

	}
	
	
}
