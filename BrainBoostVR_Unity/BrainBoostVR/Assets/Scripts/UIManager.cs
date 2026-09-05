using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Firebase.Auth;

public class UIManager : MonoBehaviour
{
    public FinalGamePanel finalPanel;
    public GameObject sessionHistoryPanel;
    public SessionHistoryPanel sessionHistoryPanelScript;

    [Header("Session")]
    public ScoreManager scoreManager;

    public void ToggleSessionHistory()
    {
        if (sessionHistoryPanelScript != null)
        {
            sessionHistoryPanelScript.OpenPanel();
        }
    }

    public void RestartGame()
    {
        if (finalPanel != null)
            finalPanel.Restart();
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Menu Principal");
    }

    public void QuitGame()
	{
		Debug.Log("[UI] Quitter le jeu...");

		if (scoreManager != null && scoreManager.sessionRunning)
		{
			scoreManager.EndSession();
		}
		else
		{
			Debug.Log("[UI] Aucune session active.");
		}

		Application.Quit();
	}

    public void CloseSessionHistory()
    {
        if (sessionHistoryPanel != null)
            sessionHistoryPanel.SetActive(false);

        if (finalPanel != null)
            finalPanel.gameObject.SetActive(true);
    }
}