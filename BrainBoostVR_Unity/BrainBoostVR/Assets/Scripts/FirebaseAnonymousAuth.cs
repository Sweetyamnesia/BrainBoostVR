using Firebase.Auth;
using UnityEngine;
using System.Threading.Tasks;

public class FirebaseAnonymousAuth : MonoBehaviour
{
    private FirebaseAuth auth;

    public static string UserId { get; private set; }
    public static string IdToken { get; private set; }


	private void Awake()
	{
		DontDestroyOnLoad(gameObject);
	}

	private async void Start()
	{
		auth = FirebaseAuth.DefaultInstance;
		await InitializeFirebaseUser();
	}

    private async Task InitializeFirebaseUser()
    {
        if (auth.CurrentUser != null)
        {
            // Utilisateur déjà connecté (même après avoir quitté le jeu)
            UserId = auth.CurrentUser.UserId;
            IdToken = await auth.CurrentUser.TokenAsync(true);
            Debug.Log($"[Firebase] Utilisateur existant : {UserId}");
        }
        else
        {
            await SignInAnonymously();
        }
    }

    private async Task SignInAnonymously()
    {
        try
        {
            var userCredential = await auth.SignInAnonymouslyAsync();
            var user = userCredential.User;
            UserId = user.UserId;
            IdToken = await user.TokenAsync(true);

            Debug.Log($"[Firebase] Nouvel utilisateur anonyme : {UserId}");
        }
        catch (System.Exception e)
        {
            Debug.LogError("[Firebase] Erreur de connexion anonyme : " + e.Message);
        }
    }
}
