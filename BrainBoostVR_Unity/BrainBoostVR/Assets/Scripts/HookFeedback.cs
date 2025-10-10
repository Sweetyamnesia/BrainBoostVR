using UnityEngine;

public class Hook : MonoBehaviour
{
    public string expectedObjectName;
    public AudioSource audioSource;
    public AudioClip correctSound;
    public AudioClip wrongSound;

    public Renderer hookRenderer; // le renderer pour changer la couleur
    public Color correctColor = Color.blue;
    public Color wrongColor = Color.red;
    public Color defaultColor = Color.gray;

    private bool isObjectPlacedCorrectly = false;

    private void Start()
    {
        if (hookRenderer != null)
            hookRenderer.material.color = defaultColor;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isObjectPlacedCorrectly) return;

        if (other.gameObject.name.Contains(expectedObjectName))
        {
            // Objet correct
            if (hookRenderer != null)
                hookRenderer.material.color = correctColor;

            audioSource.PlayOneShot(correctSound);
            isObjectPlacedCorrectly = true;
        }
        else
        {
            // Objet incorrect
            if (hookRenderer != null)
                hookRenderer.material.color = wrongColor;

            audioSource.PlayOneShot(wrongSound);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!isObjectPlacedCorrectly && hookRenderer != null)
        {
            hookRenderer.material.color = defaultColor; // revenir à la couleur de base
        }
    }
}
