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
	
	// Profil actuellement sélectionné dans le menu
	public static int CurrentUserID { get; private set; }
	public static string CurrentFirebaseUID { get; private set; }
	public static string CurrentUserName { get; private set; }

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

		statusText.text = "<color=orange>Recherche du profil...</color>";

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

			if (!FirebaseAnonymousAuth.IsTokenReady)
			{
				statusText.text = "<color=red>Firebase n'est pas encore prêt.</color>";
				return;
			}

			statusText.text = "<color=orange>Connexion à l'API...</color>";

			// Récupération de l'utilisateur Firebase actuel
			var firebaseUser =
				Firebase.Auth.FirebaseAuth.DefaultInstance.CurrentUser;

			if (firebaseUser == null)
			{
				statusText.text =
					"<color=red>Erreur : utilisateur Firebase introuvable.</color>";
				return;
			}

			string firebaseUID = firebaseUser.UserId;
			string idToken = await firebaseUser.TokenAsync(false);

			Debug.Log(
				$"[MainMenu] Création/récupération profil : {pseudo} / {firebaseUID}"
			);

			// Création ou récupération du profil utilisateur
			var userResponse = await ApiClient.CreateOrGetUserAsync(
				firebaseUID,
				pseudo,
				idToken
			);

			if (userResponse == null)
			{
				statusText.text =
					"<color=red>Impossible de contacter l'API.</color>";
				return;
			}

			// Nouveau profil créé
			if (userResponse.status == "created")
			{
				Debug.Log(
					$"[MainMenu] Nouveau profil créé : " +
					$"{userResponse.userID} / {userResponse.name}"
				);

				statusText.text =
					$"<color=green>Profil {userResponse.name} créé ✔</color>";

				// Mémoriser le profil créé
				CurrentUserID = userResponse.userID;
				CurrentFirebaseUID = userResponse.firebaseUID;
				CurrentUserName = userResponse.name;

				Debug.Log(
					$"[MainMenu] Profil courant mémorisé : " +
					$"userID={CurrentUserID}, " +
					$"name={CurrentUserName}, " +
					$"firebaseUID={CurrentFirebaseUID}"
				);

				// Entrer directement dans le jeu
				await LoadGameScene();
			}

			// Le pseudo existe déjà
			else if (userResponse.status == "pseudo_exists")
			{
				Debug.Log(
					$"[MainMenu] Le pseudo existe déjà : " +
					$"{userResponse.userID} / {userResponse.name}"
				);

				// On garde les informations du profil trouvé
				int existingUserID = userResponse.userID;
				string existingUserName = userResponse.name;

				statusText.text =
					"<color=orange>Profil existant trouvé.</color>";

				if (confirmPanel == null)
				{
					Debug.LogError(
						"[MainMenu] ConfirmPanel n'est pas assigné dans l'Inspector."
					);

					statusText.text =
						"<color=red>Erreur : panneau de confirmation manquant.</color>";

					return;
				}

				// Demander à l'utilisateur s'il souhaite utiliser ce profil
				confirmPanel.Open(
					$"Le profil \"{existingUserName}\" existe déjà.\n\n" +
					"Voulez-vous jouer avec ce profil ?",

					// OUI
					async () =>
					{
						try
						{
							statusText.text =
								"<color=orange>Association du profil...</color>";

							Debug.Log(
								$"[MainMenu] Association du profil existant : " +
								$"UserID={existingUserID}, " +
								$"FirebaseUID={firebaseUID}"
							);

							var linkResponse =
								await ApiClient.LinkProfileAsync(
									firebaseUID,
									existingUserID,
									idToken
								);

							if (linkResponse == null)
							{
								statusText.text =
									"<color=red>Impossible d'associer le profil.</color>";

								return;
							}

							if (linkResponse.status != "linked" &&
								linkResponse.status != "already_linked")
							{
								statusText.text =
									"<color=red>Erreur lors de l'association du profil.</color>";

								Debug.LogError(
									$"[MainMenu] Statut liaison inattendu : " +
									$"{linkResponse.status}"
								);

								return;
							}

							// Le profil existant est maintenant sélectionné
							CurrentUserID = existingUserID;
							CurrentFirebaseUID = firebaseUID;
							CurrentUserName = existingUserName;

							Debug.Log(
								$"[MainMenu] Profil existant sélectionné : " +
								$"userID={CurrentUserID}, " +
								$"name={CurrentUserName}, " +
								$"firebaseUID={CurrentFirebaseUID}"
							);

							statusText.text =
								$"<color=green>Profil {existingUserName} sélectionné ✔</color>";

							await LoadGameScene();
						}
						catch (System.Exception ex)
						{
							statusText.text =
								$"<color=red>Erreur : {ex.Message}</color>";

							Debug.LogError(
								"[MainMenu] Erreur association profil : " + ex
							);
						}
					},

					// NON
					() =>
					{
						statusText.text =
							"<color=orange>Veuillez choisir un autre pseudo.</color>";

						Debug.Log(
							"[MainMenu] L'utilisateur a refusé d'utiliser le profil existant."
						);
					}
				);

				return;
			}

			// Réponse API inattendue
			else
			{
				statusText.text =
					"<color=red>Réponse API inattendue.</color>";

				Debug.LogError(
					$"[MainMenu] Statut API inattendu : {userResponse.status}"
				);

				return;
			}
		}
		catch (System.Exception ex)
		{
			statusText.text =
				$"<color=red>Erreur : {ex.Message}</color>";

			Debug.LogError(
				"[MainMenu] PlayGame exception: " + ex
			);
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
