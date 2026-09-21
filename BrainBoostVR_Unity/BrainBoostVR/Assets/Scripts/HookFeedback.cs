using UnityEngine;

[RequireComponent(typeof(Collider))]
public class HookFinal : MonoBehaviour
{
    [Header("Hook Settings")]
    public string expectedObjectName;  
    public Renderer hookRenderer;
    public Color hoverCorrectColor = new Color(0f, 0f, 1f, 0.6f);
    public Color hoverWrongColor = new Color(1f, 0f, 0f, 0.6f);

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
            hookRenderer.material.color = Color.clear; // 👈 aucun halo par défaut
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

        float dist = Vector3.Distance(playerHead.position, transform.position);
        bool shouldBeVisible = dist < appearDistance;

        // 🔹 Si l’objet est déjà placé, le halo doit disparaître définitivement
        if (isPlaced)
        {
            hookRenderer.enabled = false;
            return;
        }

		if (shouldBeVisible != isVisible)
		{
			isVisible = shouldBeVisible;
			hookRenderer.enabled = isVisible;
		}
		
		// 👇 Correction ici : dès qu’on active, on force une couleur invisible
        if (isVisible)
            hookRenderer.material.color = Color.clear;
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
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("ExerciseObject")) return;
        if (other.gameObject == currentObject)
            currentObject = null;

        // 👇 Si on s’éloigne du hook, on retire toute couleur
        if (hookRenderer != null && !isPlaced)
            hookRenderer.material.color = Color.clear;

        hasPlayedSound = false;
    }

    public void TryPlaceObject(GameObject obj)
    {
        if (isPlaced || obj != currentObject) return;

        bool isCorrect = obj.name.Contains(expectedObjectName);

		// 🔊 Jouer le son correspondant
		if (!hasPlayedSound && audioSource != null)
		{
			audioSource.PlayOneShot(isCorrect ? correctSound : wrongSound);
			hasPlayedSound = true;
		}
		
		if (!isCorrect)
    	{
        	if(exerciseManager != null && exerciseManager.scoreManager != null)
            	exerciseManager.scoreManager.RegisterError();
    	}

        // 💡 Changer la couleur temporairement
        if (hookRenderer != null)
            hookRenderer.material.color = isCorrect ? hoverCorrectColor : hoverWrongColor;

        if (isCorrect)
        {
            // ✅ Marquer l'objet comme placé
            isPlaced = true;

            // 📍 Fixer l’objet à la bonne position
            obj.transform.position = transform.position;
            obj.transform.rotation = transform.rotation;

            // 🚫 Empêcher qu’il soit repris
            var grab = obj.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
            if (grab != null)
                grab.enabled = false;

            Rigidbody rb = obj.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
                rb.useGravity = false;
            }

            // ⚡ Mettre à jour le manager
            if (exerciseManager != null)
            {
                var exObj = exerciseManager.exerciseObjects.Find(x => x.objectRef == obj);
                if (exObj != null)
                    exObj.isPlacedCorrectly = true;

                if (exerciseManager.scoreManager != null)
                    exerciseManager.scoreManager.AddPoints(1);
            }

            // 🟦 Cacher le halo après une courte durée
            if (hookRenderer != null)
                Invoke(nameof(HideHaloAfterPlacement), 0.5f);
        }
    }

    private void HideHaloAfterPlacement()
    {
        if (hookRenderer != null)
        {
            hookRenderer.material.color = Color.clear;
            hookRenderer.enabled = false;
        }
    }
}
