using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Hook : MonoBehaviour
{
    [Header("Hook settings")]
    public string expectedObjectName;   // Nom attendu de l'objet
    public Renderer hookRenderer;       // Renderer du hook pour changer la couleur
    public Color defaultColor = new Color(1, 1, 1, 0f);
    public Color hoverCorrectColor = new Color(0, 0, 1, 0.6f);
	public Color hoverWrongColor = new Color(1, 0, 0, 0.6f);

    [Header("Audio")]
    public AudioSource audioSource;     // AudioSource attachée au hook ou à un GameObject parent
    public AudioClip correctSound;
	public AudioClip wrongSound;
	
	[Header("Manager")]
	public ExerciseManager exerciseManager;

	private GameObject currentObject;   // Objet actuellement proche du hook
	private bool hasPlayedSound = false;
	private bool isObjectPlaced = false;

    private void Start()
    {
        if (hookRenderer != null)
            hookRenderer.material.color = defaultColor;
    }

    private void OnTriggerEnter(Collider other)
    {
		if (!other.CompareTag("ExerciseObject")) return;

		currentObject = other.gameObject;
        UpdateHookColor();
    }

    private void OnTriggerExit(Collider other)
    {
		if (!other.CompareTag("ExerciseObject")) return;
		currentObject = null;
		hasPlayedSound = false;	

		if (hookRenderer != null)
            hookRenderer.material.color = defaultColor;
    }

    // Appeler cette fonction quand l’objet est relâché
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

        // Fixer la couleur finale après placement
        if (hookRenderer != null)
            hookRenderer.material.color = isCorrect ? hoverCorrectColor : hoverWrongColor;

		// Mettre à jour l'ExerciseManager
		if (isCorrect && exerciseManager != null)
		{
			var exerciseObj = exerciseManager.exerciseObjects.Find(x => x.objectRef == obj);
			if (exerciseObj != null)
				exerciseObj.isPlacedCorrectly = true;
		}
		isObjectPlaced = isCorrect;	
    }

    private void UpdateHookColor()
    {
        if (hookRenderer == null || currentObject == null) return;

        bool isCorrect = currentObject.name.Contains(expectedObjectName);
        hookRenderer.material.color = isCorrect ? hoverCorrectColor : hoverWrongColor;
    }
}