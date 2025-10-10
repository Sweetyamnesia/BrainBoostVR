using UnityEngine;

public class HookFeedback : MonoBehaviour
{
	[Header("Hook Settings")]
	public string expectedObjectName; //le nom de l'objet attendu (ex: Banana, Laptop...)
	public Color correctColor = Color.blue;
	public Color wrongColor = Color.red;
	public Color defaultColor = Color.gray;

	[Header("Audio Feedback")]
	public AudioSource audioSource;
	public AudioClip correctSound;
	public AudioClip wrongSound;

	private Renderer rend;

	void Start()
	{
		rend = GetComponent<Renderer>();
		if (rend != null)
		{
			rend.material.color = defaultColor;
		}
		else
		{
			Debug.LogWarning("[HOOK] Aucun Renderer trouvé sur (name)");
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		string objectName = other.gameObject.name;
		if (objectName.Contains(expectedObjectName))
		{
			rend.material.color = correctColor;

			if (audioSource != null && correctSound != null)
				audioSource.PlayOneShot(correctSound);
		}
		else
		{
			rend.material.color = wrongColor;

			if (audioSource != null && correctSound != null)
				audioSource.PlayOneShot(correctSound);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (rend == null) return;
		rend.material.color = defaultColor;		
	}
}
