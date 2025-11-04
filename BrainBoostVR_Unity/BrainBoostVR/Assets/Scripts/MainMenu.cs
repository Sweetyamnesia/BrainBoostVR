using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Threading.Tasks;

public class MainMenu : MonoBehaviour
{
    [Header("UI")]
    public TMP_InputField pseudoInput;
    public TextMeshProUGUI pseudoErrorText;

    public GameObject confirmPanel;
    public TextMeshProUGUI confirmText;

    [Header("Audio")]
    public GameObject menuAmbiance; // GameObject avec AudioFade

    private void Start()
    {
        // Fade in du menu
        if (menuAmbiance != null)
            menuAmbiance.GetComponent<AudioFade>().FadeIn();

        // Masquer les messages et panneau au départ
        pseudoErrorText.text = "";
        confirmPanel.SetActive(false);
    }

    public async void PlayGame()
    {
        string pseudo = pseudoInput.text.Trim();

        if (string.IsNullOrEmpty(pseudo))
        {
            pseudoErrorText.text = "<color=red>Veuillez entrer un pseudo.</color>";
            return;
        }

        pseudoErrorText.text = "";

        // Récupérer le token Firebase
        string idToken = FirebaseAnonymousAuth.IdToken;

        // Créer ou récupérer l'utilisateur via l'API
        string firebaseUID = await ApiClient.CreateOrGetUserAsync(pseudo, idToken);

        if (string.IsNullOrEmpty(firebaseUID))
        {
            pseudoErrorText.text = "<color=red>Erreur lors de la connexion, réessayez.</color>";
            return;
        }

        // Vérifier si l'utilisateur est existant ou nouveau
        if (firebaseUID == "EXISTS")
        {
            confirmText.text = $"Le pseudo <b>{pseudo}</b> existe déjà.\nVoulez-vous continuer avec ce profil ?";
            confirmPanel.SetActive(true);
            return;
        }
        else if (firebaseUID == "NEW")
        {
            pseudoErrorText.text = "<color=green>Profil créé avec succès !</color>";
        }

        LaunchGame();
    }

    // Bouton "Oui" dans le confirmPanel
    public void OnConfirmYes()
    {
        confirmPanel.SetActive(false);
        LaunchGame();
    }

    // Bouton "Non" dans le confirmPanel
    public void OnConfirmNo()
    {
        confirmPanel.SetActive(false);
        pseudoInput.text = "";
        pseudoErrorText.text = "Choisissez un autre pseudo.";
    }

    // Méthode pour fade audio et charger la scène principale
    private void LaunchGame()
    {
        if (menuAmbiance != null)
            menuAmbiance.GetComponent<AudioFade>().FadeOut();

        SceneManager.LoadScene("BrainBoostVR");
    }

    public void OpenTutorial()
    {
        if (menuAmbiance != null)
            menuAmbiance.GetComponent<AudioFade>().FadeOut();

        SceneManager.LoadScene("TutorialScene");
    }

    public void QuitGame()
    {
        if (menuAmbiance != null)
            menuAmbiance.GetComponent<AudioFade>().FadeOut();

        Debug.Log("Quitter le jeu");
        Application.Quit();
    }
}
