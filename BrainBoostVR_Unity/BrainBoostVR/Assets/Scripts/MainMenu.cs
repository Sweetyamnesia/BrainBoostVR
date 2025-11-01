using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Audio")]
    public GameObject menuAmbiance; // Assigner le GameObject avec AudioFade

    private void Start()
    {
        // Quand le menu apparaît, fade in de l'ambiance
        if(menuAmbiance != null)
        {
            menuAmbiance.GetComponent<AudioFade>().FadeIn();
        }
    }

    public void PlayGame()
    {
        if(menuAmbiance != null)
        {
            menuAmbiance.GetComponent<AudioFade>().FadeOut();
        }
        SceneManager.LoadScene("BrainBoostVR"); // Scène principale
    }

    public void OpenTutorial()
    {
        if(menuAmbiance != null)
        {
            menuAmbiance.GetComponent<AudioFade>().FadeOut();
        }
        SceneManager.LoadScene("TutorialScene"); // Scène tutoriel
    }

    public void QuitGame()
    {
        if(menuAmbiance != null)
        {
            menuAmbiance.GetComponent<AudioFade>().FadeOut();
        }
        Debug.Log("Quitter le jeu");
        Application.Quit();
    }
}
