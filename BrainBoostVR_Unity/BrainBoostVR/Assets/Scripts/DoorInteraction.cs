using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DoorController : MonoBehaviour
{
    [Header("References")]
    public Transform leftDoor;
    public Transform rightDoor;
    public float openAngle = 90f;       // Angle d’ouverture
    public float openSpeed = 2f;        // Vitesse de rotation

    public UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable leftGrab;
    public UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable rightGrab;

    private bool doorOpened = false;
    private float leftCurrentAngle = 0f;
    private float rightCurrentAngle = 0f;
    private bool opening = false;

	public ExerciseManager exerciseManager;


    void Start()
    {
        if (leftGrab != null)
            leftGrab.selectEntered.AddListener(OnDoorGrabbed);

        if (rightGrab != null)
            rightGrab.selectEntered.AddListener(OnDoorGrabbed);
    }

    private void OnDoorGrabbed(SelectEnterEventArgs args)
    {
        if (!doorOpened)
        {
            doorOpened = true;
            opening = true;

            Debug.Log("[DOOR] Porte attrapée !");
            TriggerScenario();
        }
    }

	private void TriggerScenario()
	{
    	Debug.Log("[SCENARIO] Début du scénario principal !");

    	if (exerciseManager != null)
    	{
        	exerciseManager.StartExercise();
        	Debug.Log("[SCENARIO] Exercice lancé !");
    	}
    	else
    	{
        	Debug.LogWarning("[SCENARIO] Aucun ExerciseManager assigné !");
    	}
	}

    void Update()
    {
        if (opening)
        {
            // Rotation gauche
            if (leftCurrentAngle < openAngle)
            {
                float step = openSpeed * Time.deltaTime;
                leftDoor.Rotate(Vector3.up, step);
                leftCurrentAngle += step;
            }

            // Rotation droite (inverse)
            if (rightCurrentAngle < openAngle)
            {
                float step = openSpeed * Time.deltaTime;
                rightDoor.Rotate(Vector3.up, -step);
                rightCurrentAngle += step;
            }

            // Si les deux portes ont fini de tourner
            if (leftCurrentAngle >= openAngle && rightCurrentAngle >= openAngle)
            {
                opening = false;
            }
        }
    }
}
