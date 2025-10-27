using UnityEngine;
using TMPro;

public class FinalGamePanel : MonoBehaviour
{
    [Header("UI Texts")]
    public TextMeshProUGUI textScore;
    public TextMeshProUGUI textError;
    public TextMeshProUGUI textTemps;

    [Header("Audio")]
    public AudioSource audioSource;
	public AudioClip clickSound;

	public GameObject sessionHistoryPanel;

	public void DisplayEnd(int score, int errors, float temps)
	{
		if (textScore != null)
			textScore.text = "Score final: " + score;

		if (textError != null)
			textError.text = "Erreurs: " + errors;

		if (textTemps != null)
		{
			int minutes = Mathf.FloorToInt(temps / 60);
			int secondes = Mathf.FloorToInt(temps % 60);
			textTemps.text = $"Temps passé: {minutes:00}:{secondes:00}";
		}

		gameObject.SetActive(true);
	}
	
	public void ShowSessionHistory()
	{
		PlayClick();
		if (sessionHistoryPanel != null)
			sessionHistoryPanel.SetActive(true);
	}

    public void Restart()
    {
        PlayClick();

        // Réinitialiser le ScoreManager
        ScoreManager scoreManager = Object.FindFirstObjectByType<ScoreManager>();
        if (scoreManager != null)
            scoreManager.ResetScore();

        // Réinitialiser ExerciseManager
        ExerciseManager exerciseManager = FindFirstObjectByType<ExerciseManager>();
        if (exerciseManager != null)
            exerciseManager.StartExercise();

        // Fermer le panneau final
        gameObject.SetActive(false);
    }

    private void PlayClick()
    {
        if (audioSource != null && clickSound != null)
            audioSource.PlayOneShot(clickSound);
    }
}
