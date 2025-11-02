using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

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

    // ======================
    //   DISPLAY FINAL PANEL
    // ======================
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

    // ======================
    //   SESSION HISTORY
    // ======================
    public void ShowSessionHistory()
    {
        PlayClick();
        if (sessionHistoryPanel != null)
            sessionHistoryPanel.SetActive(true);
    }

    // ======================
    //   RESTART GAME
    // ======================
    public void Restart()
    {
        PlayClick();

        // Fermer le panneau final (facultatif mais plus propre)
        gameObject.SetActive(false);

        // Recharge la scène active pour tout réinitialiser
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    // ======================
    //   AUDIO CLICK
    // ======================
    private void PlayClick()
    {
        if (audioSource != null && clickSound != null)
            audioSource.PlayOneShot(clickSound);
    }
}
