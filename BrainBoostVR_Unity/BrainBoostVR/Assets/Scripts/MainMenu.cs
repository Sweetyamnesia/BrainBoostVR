using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [Header("UI")]
    public TMP_InputField pseudoInput;
    public TextMeshProUGUI pseudoErrorText;
	
	[Header("Audio")]
    public GameObject menuAmbiance; // Assigner le GameObject avec AudioFade

    private void Start()
    {
        // Quand le menu apparaît, fade in de l'ambiance
        if (menuAmbiance != null)
        {
			menuAmbiance.GetComponent<AudioFade>().FadeIn();
			pseudoErrorText.text = "";
        }
    }

    public void PlayGame()
	{
        string pseudo = pseudoInput.text.Trim();

        if (string.IsNullOrEmpty(pseudo))
        {
            pseudoErrorText.text = "Veuillez entrer un pseudo.";
            return; // bloque le lancement du jeu
        }

        pseudoErrorText.text = "";
		
		if (menuAmbiance != null)
        {
            menuAmbiance.GetComponent<AudioFade>().FadeOut();
        }
        SceneManager.LoadScene("BrainBoostVR"); // Scène principale
    }

    public void OpenTutorial()
    {
        if (menuAmbiance != null)
        {
            menuAmbiance.GetComponent<AudioFade>().FadeOut();
        }
        SceneManager.LoadScene("TutorialScene"); // Scène tutoriel
    }

    public void QuitGame()
    {
        if (menuAmbiance != null)
        {
            menuAmbiance.GetComponent<AudioFade>().FadeOut();
        }
        Debug.Log("Quitter le jeu");
        Application.Quit();
    }
}
