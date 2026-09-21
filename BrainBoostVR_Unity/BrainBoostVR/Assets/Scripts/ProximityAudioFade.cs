using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ProximityAudioFade : MonoBehaviour
{
    [Header("Player Settings")]
    public Transform player;                // XR Rig ou caméra du joueur
    public float activationDistance = 3f;   // Distance pour activer le son
    public float fadeSpeed = 2f;            // Vitesse du fade

    private AudioSource audioSource;
    private float targetVolume;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource manquant sur " + gameObject.name);
        }
        audioSource.volume = 0f;
        audioSource.loop = true;
        audioSource.Play();
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(player.position, transform.position);

        // Définir le volume cible en fonction de la distance
        targetVolume = distance <= activationDistance ? 1f : 0f;

        // Lisser le volume
        audioSource.volume = Mathf.MoveTowards(audioSource.volume, targetVolume, fadeSpeed * Time.deltaTime);
    }
}
