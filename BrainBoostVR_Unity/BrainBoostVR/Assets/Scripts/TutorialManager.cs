using UnityEngine;
using TMPro;
using System.Collections;

[System.Serializable]
public class TutorialStep
{
    public string text;           // Texte à afficher
    public AudioClip audio;       // Audio de la consigne
    public GameObject targetZone; // Zone à activer
}

public class TutorialManager : MonoBehaviour
{
    
	public int CurrentStepIndex => currentStep;

	public TutorialStep[] steps;
    public TextMeshProUGUI subtitleText;
    public AudioSource audioSource;

    private int currentStep = 0;
    private bool waitingForAction = false;

    void Start()
    {
        if (steps.Length > 0)
            StartCoroutine(PlayStep(currentStep));
    }

    IEnumerator PlayStep(int stepIndex)
    {
        TutorialStep step = steps[stepIndex];

        // Activer la zone
        if (step.targetZone != null)
            step.targetZone.SetActive(true);

        // Jouer audio
        if (step.audio != null && audioSource != null)
        {
            audioSource.clip = step.audio;
            audioSource.Play();
        }

        // Afficher texte
        if (subtitleText != null)
            subtitleText.text = step.text;

        // Attendre la fin de l’audio avant de permettre de continuer
        if (audioSource != null)
            yield return new WaitForSeconds(audioSource.clip.length);

        // Le script attend maintenant que le joueur fasse une action
        waitingForAction = true;
    }

    public void NextStep()
    {
        if (!waitingForAction) return;

        // Désactiver l’ancienne zone
        if (steps[currentStep].targetZone != null)
            steps[currentStep].targetZone.SetActive(false);

        waitingForAction = false;
        currentStep++;

        if (currentStep < steps.Length)
            StartCoroutine(PlayStep(currentStep));
        else
            EndTutorial();
    }

    void EndTutorial()
	{
		if (subtitleText != null)
		subtitleText.text = "Bravo, vous avez terminé le tutoriel !";
        
		Debug.Log("Tutoriel terminé !");
        StartCoroutine(ReturnToMenu());
    }

    IEnumerator ReturnToMenu()
    {
        yield return new WaitForSeconds(5f);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu Principal");
    }
}
