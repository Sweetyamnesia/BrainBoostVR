using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Footsteps : MonoBehaviour
{
    public AudioSource footstepAudio;
    public float stepInterval = 0.5f; // temps entre deux pas
    private float stepTimer;

    private CharacterController characterController; // si tu utilises un CharacterController

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        stepTimer = stepInterval;
    }

    void Update()
    {
        if (characterController == null) return;

        // Vérifie si le joueur bouge horizontalement
        Vector3 horizontalVelocity = new Vector3(characterController.velocity.x, 0, characterController.velocity.z);
        if (horizontalVelocity.magnitude > 0.1f)
        {
            stepTimer -= Time.deltaTime;
            if (stepTimer <= 0f)
            {
                footstepAudio.Play();
                stepTimer = stepInterval;
            }
        }
        else
        {
            stepTimer = stepInterval; // reset si immobile
        }
    }
}
