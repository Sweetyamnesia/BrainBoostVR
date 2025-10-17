using UnityEngine;

[RequireComponent(typeof(Collider))]
public class HookFinal : MonoBehaviour
{
    [Header("Hook Settings")]
    public string expectedObjectName;  
    public Renderer hookRenderer;
    public Color hoverCorrectColor = new Color(0f, 0, 1f, 0.6f);
    public Color hoverWrongColor = new Color(1f, 0, 0, 0.6f);

    [Header("Audio")]
    public AudioSource audioSource;     
    public AudioClip correctSound;
    public AudioClip wrongSound;

    [Header("Visibility")]
    public Transform playerHead;
    public float appearDistance = 1.2f;

    [Header("Managers")]
    public ExerciseManager exerciseManager;

    private GameObject currentObject;
    private bool hasPlayedSound = false;
    private bool isPlaced = false;
    private bool isVisible = false;

    private void Start()
    {
        if (hookRenderer != null)
        {
            hookRenderer.enabled = false;
        }

        Collider col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    private void Update()
    {
        UpdateVisibility();
        UpdateHaloColor();
    }

    private void UpdateVisibility()
    {
        if (playerHead == null || hookRenderer == null) return;
        if (isPlaced) return; // 🔹 Ne rien faire si déjà placé

        float dist = Vector3.Distance(playerHead.position, transform.position);
        bool shouldBeVisible = dist < appearDistance && currentObject != null;

        if (shouldBeVisible != isVisible)
        {
            isVisible = shouldBeVisible;
            hookRenderer.enabled = isVisible;
        }
    }

    private void UpdateHaloColor()
    {
        if (!isVisible || currentObject == null || isPlaced) return;

        bool isCorrect = currentObject.name.Contains(expectedObjectName);
        hookRenderer.material.color = isCorrect ? hoverCorrectColor : hoverWrongColor;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("ExerciseObject")) return;
        if (isPlaced) return;

        currentObject = other.gameObject;
        hasPlayedSound = false;

        // ✅ Active le halo seulement à l’entrée de l’objet
        if (hookRenderer != null)
        {
            hookRenderer.enabled = true;
            bool isCorrect = currentObject.name.Contains(expectedObjectName);
            hookRenderer.material.color = isCorrect ? hoverCorrectColor : hoverWrongColor;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("ExerciseObject")) return;
        if (other.gameObject == currentObject)
            currentObject = null;

        hasPlayedSound = false;

        // 🔹 Désactive le halo quand l’objet quitte le trigger
        if (hookRenderer != null && !isPlaced)
            hookRenderer.enabled = false;
    }

    public void TryPlaceObject(GameObject obj)
    {
        if (isPlaced || obj != currentObject) return;

        bool isCorrect = obj.name.Contains(expectedObjectName);

        // 🔊 Son correct / incorrect
        if (!hasPlayedSound && audioSource != null)
        {
            audioSource.PlayOneShot(isCorrect ? correctSound : wrongSound);
            hasPlayedSound = true;
        }

        // 💡 Couleur finale du halo
        if (hookRenderer != null)
        {
            hookRenderer.material.color = isCorrect ? hoverCorrectColor : hoverWrongColor;
        }

        if (isCorrect)
        {
            // ✅ Objet placé correctement
            isPlaced = true;

            obj.transform.position = transform.position;
            obj.transform.rotation = transform.rotation;

            Rigidbody rb = obj.GetComponent<Rigidbody>();
			if (rb != null) rb.isKinematic = true;
			
			var grab = obj.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
			if (grab != null) grab.enabled = false;

            // 🔹 Cache le halo après placement
            if (hookRenderer != null)
                hookRenderer.enabled = false;

            // ⚡ Mise à jour du manager
            if (exerciseManager != null)
            {
                var exerciseObj = exerciseManager.exerciseObjects.Find(x => x.objectRef == obj);
                if (exerciseObj != null)
                    exerciseObj.isPlacedCorrectly = true;
            }
        }
    }
}
