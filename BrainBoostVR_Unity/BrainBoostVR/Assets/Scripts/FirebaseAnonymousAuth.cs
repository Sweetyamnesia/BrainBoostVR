using Firebase;
using Firebase.Auth;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.Networking;
using System; 

public class FirebaseAnonymousAuth : MonoBehaviour
{
    private FirebaseAuth auth;

    public static string UserId { get; private set; }
    public static string IdToken { get; private set; }
    public static bool IsTokenReady { get; private set; } = false;

    [Header("API Settings")]
    [Tooltip("Adresse de ton API locale ou distante")]
    public string apiBaseUrl = "http://10.5.2.38:5286/api"; // 🔹 change selon ton réseau

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private async void Start()
	{
        var status = await Firebase.FirebaseApp.CheckAndFixDependenciesAsync();
		if (status != Firebase.DependencyStatus.Available)
		{
			Debug.LogError($"Firebase non disponible: {status}");
			return;
		}

		await InitializeFirebase();
        await InitializeFirebaseUser();
    }

    private async Task InitializeFirebase()
    {
        var dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();
        if (dependencyStatus != DependencyStatus.Available)
        {
            Debug.LogError($"[Firebase] Dépendances manquantes : {dependencyStatus}");
        }
        else
        {
            Debug.Log("[Firebase] Firebase initialisé ✅");
        }
    }

    private async Task InitializeFirebaseUser()
    {
        auth = FirebaseAuth.DefaultInstance;

        try
        {
            if (auth.CurrentUser != null)
            {
                UserId = auth.CurrentUser.UserId;
                IdToken = await auth.CurrentUser.TokenAsync(true);
                IsTokenReady = true;
                Debug.Log($"[Firebase] Utilisateur existant : {UserId}");
            }
            else
            {
                await SignInAnonymously();
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[Firebase] Erreur initialisation user : {e.Message}");
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
            IsTokenReady = true;
            Debug.Log($"[Firebase] Nouvel utilisateur anonyme : {UserId}");
        }
        catch (System.Exception e)
        {
            Debug.LogError("[Firebase] Erreur connexion anonyme : " + e.Message);
            IsTokenReady = false;
        }
    }

    // 🔹 Test envoi du token vers l'API
    public async Task TestApiConnection()
	{
		if (auth == null)
		{
			Debug.LogError("[Unity] FirebaseAuth non initialisé.");
			return;
		}

		// 🔹 Récupère un token frais
		var currentUser = auth.CurrentUser;
		if (currentUser == null)
		{
			Debug.LogError("[Unity] Aucun utilisateur connecté.");
			return;
		}

		IdToken = await currentUser.TokenAsync(true); // ✅ true = force refresh
		Debug.Log("[Unity] Token Firebase reçu et rafraîchi ✅");
		Debug.Log($"[Unity] Timestamp token : {DateTime.UtcNow}"); 
		Debug.Log($"[Unity] Token envoyé : {IdToken}");

		string testUrl = $"{apiBaseUrl}/api/test";

		using (UnityWebRequest request = new UnityWebRequest(testUrl, "GET"))
		{
			request.downloadHandler = new DownloadHandlerBuffer();
			request.SetRequestHeader("Authorization", "Bearer " + IdToken);

			Debug.Log($"[Unity] URL test : {testUrl}");
			Debug.Log($"[Unity] Token envoyé : {IdToken}");

			var operation = request.SendWebRequest();

			while (!operation.isDone)
				await Task.Yield();

			Debug.Log($"[Unity] Résultat requête : {request.result}");
			Debug.Log($"[Unity] Code HTTP: {request.responseCode}");

			if (request.result == UnityWebRequest.Result.Success)
				Debug.Log($"[Unity] Réponse API : {request.downloadHandler.text}");
			else
			{
				Debug.LogError($"[Unity] Erreur API : {request.error}");
				Debug.LogError($"[Unity] Contenu renvoyé : {request.downloadHandler.text}");
			}
		}
	}
}
