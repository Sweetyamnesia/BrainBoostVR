using Firebase.Auth;
using UnityEngine;
using System.Threading.Tasks;

public class FirebaseAnonymousAuth : MonoBehaviour
{
    private FirebaseAuth auth;

    private async void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
        await SignInAnonymously();
    }

    private async Task SignInAnonymously()
    {
        try
        {
            var userCredential = await auth.SignInAnonymouslyAsync();
            FirebaseUser user = userCredential.User; // ← c’est ça qu’il faut
            string idToken = await user.TokenAsync(true); // force refresh
            Debug.Log("Token anonyme (JWT) : " + idToken);

            // Ici tu peux envoyer idToken à ton API via UnityWebRequest
        }
        catch (System.Exception e)
        {
            Debug.LogError("Erreur connexion anonyme : " + e.Message);
        }
    }
}
