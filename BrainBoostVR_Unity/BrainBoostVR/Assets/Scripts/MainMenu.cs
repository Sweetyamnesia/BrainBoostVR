using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("BrainBoostVR"); // Nom exact de ta scène principale
    }

    public void OpenTutorial()
    {
        SceneManager.LoadScene("TutorialScene"); // Nom de la future scène de tuto
    }

    public void QuitGame()
    {
        Debug.Log("Quitter le jeu");
        Application.Quit();
    }
}
