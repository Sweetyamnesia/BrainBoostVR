using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Hook : MonoBehaviour
{
    [Header("Hook settings")]
    public string expectedObjectName;  
    public Renderer hookRenderer;       
    public Color defaultColor = new Color(1, 1, 1, 0f);
    public Color hoverCorrectColor = new Color(0, 0, 1, 0.6f);
    public Color hoverWrongColor = new Color(1, 0, 0, 0.6f);

    [Header("Audio")]
    public AudioSource audioSource;     
    public AudioClip correctSound;
    public AudioClip wrongSound;

    [Header("Visibility")]
    public Transform playerHead;
    public float appearDistance = 1.2f;

    [Header("Manager")]
    public ExerciseManager exerciseManager;

    private GameObject currentObject;   
    private bool hasPlayedSound = false;
    private bool isObjectPlaced = false;
    private bool isVisible = false;

    private void Start()
    {
        if (hookRenderer != null)
        {
            hookRenderer.enabled = false;
            hookRenderer.material.color = defaultColor;
        }
    }

    private void Update()
    {
        HandleVisibility();
    }

    private void HandleVisibility()
    {
        if (playerHead == null || hookRenderer == null)
            return;

        float dist = Vector3.Distance(playerHead.position, transform.position);
        bool shouldBeVisible = dist < appearDistance;

        if (shouldBeVisible != isVisible)
        {
            isVisible = shouldBeVisible;
            hookRenderer.enabled = isVisible;

            if (!isVisible)
                hookRenderer.material.color = defaultColor;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("ExerciseObject")) return;

        currentObject = other.gameObject;
        hasPlayedSound = false;

        if (hookRenderer != null)
            hookRenderer.material.color = defaultColor;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("ExerciseObject")) return;

        if (other.gameObject == currentObject)
            currentObject = null;

        hasPlayedSound = false;

        if (hookRenderer != null)
            hookRenderer.material.color = defaultColor;
    }

    public void TryPlaceObject(GameObject obj)
    {
        if (obj != currentObject) return;

        bool isCorrect = obj.name.Contains(expectedObjectName);
        Debug.Log($"[HOOK] TryPlaceObject() -> {obj.name} | correct = {isCorrect}");

        if (!hasPlayedSound && audioSource != null)
        {
            audioSource.PlayOneShot(isCorrect ? correctSound : wrongSound);
            hasPlayedSound = true;
        }

        // Couleur finale
        if (hookRenderer != null)
            hookRenderer.material.color = isCorrect ? hoverCorrectColor : hoverWrongColor;

        isObjectPlaced = isCorrect;

        // Mettre à jour ExerciseManager
        if (isCorrect && exerciseManager != null)
        {
            var exerciseObj = exerciseManager.exerciseObjects.Find(x => x.objectRef == obj);
            if (exerciseObj != null)
                exerciseObj.isPlacedCorrectly = true;
        }
    }
}
