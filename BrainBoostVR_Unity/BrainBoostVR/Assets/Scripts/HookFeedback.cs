using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Hook : MonoBehaviour
{
    [Header("Hook settings")]
    public string expectedObjectName;   // Nom attendu de l'objet
    public Renderer hookRenderer;       // Renderer du hook pour changer la couleur
    public Color defaultColor = Color.gray;
    public Color hoverCorrectColor = Color.blue;
    public Color hoverWrongColor = Color.red;

    [Header("Audio")]
    public AudioSource audioSource;     // AudioSource attachée au hook ou à un GameObject parent
    public AudioClip correctSound;
    public AudioClip wrongSound;

	private GameObject currentObject;   // Objet actuellement proche du hook
	private bool hasPlayedSound = false;
    private bool isObjectPlaced = false;

    private void Start()
    {
        if (hookRenderer != null)
            hookRenderer.material.color = defaultColor;

        if (audioSource == null)
            Debug.LogWarning("[Hook] Pas d'AudioSource assignée !");
    }

    private void OnTriggerEnter(Collider other)
    {
		if (other.CompareTag("ExerciseObject")) return;

		currentObject = other.gameObject;
		hasPlayedSound = false;
        UpdateHookColor();
    }

    private void OnTriggerStay(Collider other)
    {
		if (other.CompareTag("ExerciseObject")) return;
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
        if (currentObject == null||obj != currentObject) return;

		bool isCorrect = obj.name.Contains(expectedObjectName);
		isObjectPlaced = isCorrect;
        
		if (audioSource != null && !hasPlayedSound)
        {
            AudioClip clip = isCorrect ? correctSound : wrongSound;
			if (clip != null)
				audioSource.PlayOneShot(clip);
			hasPlayedSound = true;
        }

        // Fixer la couleur finale après placement
        if (hookRenderer != null)
            hookRenderer.material.color = isCorrect ? hoverCorrectColor : hoverWrongColor;

        isObjectPlaced = isCorrect;
    }

    private void UpdateHookColor()
    {
        if (hookRenderer == null || currentObject == null) return;

        bool isCorrect = currentObject.name.Contains(expectedObjectName);
        hookRenderer.material.color = isCorrect ? hoverCorrectColor : hoverWrongColor;
    }
}
