using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.Threading.Tasks;

public class MainMenu : MonoBehaviour
{
    [Header("UI")]
    public TMP_InputField pseudoInput;
    public TextMeshProUGUI statusText;
    public Button playButton;
    public Button tutorialButton;
    public Button quitButton;

    [Header("Audio")]
    public GameObject menuAmbiance;

    private FirebaseAnonymousAuth firebaseAuth;
    private bool isProcessing = false;

    [Header("Panel")]
    public ConfirmPanel confirmPanel;

    private async void Start()
    {
        firebaseAuth = Object.FindFirstObjectByType<FirebaseAnonymousAuth>();

        // Fade in de la musique
        menuAmbiance?.GetComponent<AudioFade>()?.FadeIn();

        // Activer tous les boutons
        SetButtonsInteractable(true);

        statusText.text = "";

        // Attendre que Firebase soit prêt
        await WaitForFirebaseReady();
    }

    private void SetButtonsInteractable(bool state)
    {
        playButton.interactable = state;
        tutorialButton.interactable = state;
        quitButton.interactable = state;
    }

    private async Task WaitForFirebaseReady()
    {
        int maxWait = 20; // 20 * 0.5s = 10s max
        while (!FirebaseAnonymousAuth.IsTokenReady && maxWait > 0)
        {
            statusText.text = "<color=orange>Connexion à Firebase...</color>";
            await Task.Delay(500);
            maxWait--;
        }

        if (FirebaseAnonymousAuth.IsTokenReady)
        {
            statusText.text = "<color=green>Firebase connecté ✔</color>";
        }
        else
        {
            statusText.text = "<color=red>Impossible de se connecter à Firebase.</color>";
        }
    }

    public async void PlayGame()
    {
        if (isProcessing) return;
        isProcessing = true;
        SetButtonsInteractable(false);

        try
        {
            if (firebaseAuth == null)
            {
                statusText.text = "<color=red>Erreur : Auth non initialisée.</color>";
                return;
            }

            string pseudo = pseudoInput.text.Trim();
            if (string.IsNullOrEmpty(pseudo))
            {
                statusText.text = "<color=red>Veuillez entrer un pseudo.</color>";
                return;
            }

            statusText.text = "<color=orange>Connexion à l'API...</color>";

            // Test de connexion à Firebase et création de session
            await firebaseAuth.TestApiConnection();

            // Chargement de la scène principale
            await LoadGameScene();
        }
        catch (System.Exception ex)
        {
            statusText.text = $"<color=red>Erreur : {ex.Message}</color>";
            Debug.LogError("[MainMenu] PlayGame exception: " + ex.Message);
        }
        finally
        {
            isProcessing = false;
            SetButtonsInteractable(true);
        }
    }

    private async Task LoadGameScene()
    {
        await Task.Delay(500); // Petit délai pour fade audio
        menuAmbiance?.GetComponent<AudioFade>()?.FadeOut();
        SceneManager.LoadScene("BrainBoostVR");
    }

    public void OpenTutorial()
    {
        menuAmbiance?.GetComponent<AudioFade>()?.FadeOut();
        SceneManager.LoadScene("TutorialScene");
    }

    public void QuitGame()
    {
        menuAmbiance?.GetComponent<AudioFade>()?.FadeOut();
        Application.Quit();
    }
}
