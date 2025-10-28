using UnityEngine;
using TMPro;
using System.Collections;

[System.Serializable]
public class TutorialStep
{
    public string text;          // texte à afficher
    public AudioClip audio;      // audio de la consigne
    public GameObject targetZone; // zone à activer
}

public class TutorialManager : MonoBehaviour
{
    public TutorialStep[] steps;
    public TextMeshProUGUI subtitleText;
    public AudioSource audioSource;

    private int currentStep = 0;

    void Start()
    {
        if(steps.Length > 0)
        {
            StartCoroutine(PlayStep(currentStep));
        }
    }

    IEnumerator PlayStep(int stepIndex)
    {
        TutorialStep step = steps[stepIndex];

        // Activer la zone
        if(step.targetZone != null)
            step.targetZone.SetActive(true);

        // Jouer audio
        if(step.audio != null && audioSource != null)
        {
            audioSource.clip = step.audio;
            audioSource.Play();
        }

        // Afficher le texte
        if(subtitleText != null)
        {
            subtitleText.text = step.text;
        }

        // Attendre la fin de l’audio
        if(audioSource != null)
            yield return new WaitForSeconds(audioSource.clip.length);
        else
            yield return new WaitForSeconds(3f); // fallback

        // Désactiver la zone
        if(step.targetZone != null)
            step.targetZone.SetActive(false);

        // Passer à l’étape suivante
        currentStep++;
        if(currentStep < steps.Length)
            StartCoroutine(PlayStep(currentStep));
        else
            EndTutorial();
    }

    void EndTutorial()
    {
        // Exemple : revenir au menu principal
        UnityEngine.SceneManagement.SceneManager.LoadScene("MenuPrincipal");
    }
}
