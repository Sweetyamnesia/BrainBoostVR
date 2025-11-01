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
        SceneManager.LoadScene("Menu Principal");
    }

	public void QuitGame()
	{
		Debug.Log("Quitter le jeu...");
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
