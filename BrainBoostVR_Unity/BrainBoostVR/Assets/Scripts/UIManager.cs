using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public FinalGamePanel finalPanel;
    public GameObject sessionHistoryPanel;

    public void ToggleSessionHistory()
    {
        if (sessionHistoryPanel != null)
            sessionHistoryPanel.SetActive(!sessionHistoryPanel.activeSelf);
    }

    public void RestartGame()
    {
        if (finalPanel != null)
            finalPanel.Restart();
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MenuPrincipal");
    }

    public void QuitGame()
    {
        Debug.Log("Quitter le jeu...");
        Application.Quit();
    }
}
