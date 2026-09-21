using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Text;
using System.Threading.Tasks;

public class SessionHistoryPanel : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI historyText;
    public Button closeButton;

    [Header("Data")]
    public ScoreManager scoreManager;

    private void Start()
    {
        if (closeButton != null)
            closeButton.onClick.AddListener(ClosePanel);

        gameObject.SetActive(false);
    }

    public async void OpenPanel()
    {
        await Task.Yield();

        if (scoreManager == null)
        {
            Debug.LogWarning(
                "[SESSION HISTORY] ScoreManager not assigned!"
            );
        }

        // --------------------------------------------------------
        // Le profil actuellement sélectionné dans le Main Menu
        // --------------------------------------------------------
        int userID = MainMenu.CurrentUserID;

        if (userID <= 0)
        {
            Debug.LogError(
                "[SESSION HISTORY] Aucun profil sélectionné."
            );

            if (historyText != null)
                historyText.text =
                    "Aucun profil utilisateur sélectionné.";

            gameObject.SetActive(true);
            return;
        }

        // --------------------------------------------------------
        // Récupération du token Firebase
        // --------------------------------------------------------
        string idToken = FirebaseAnonymousAuth.IdToken;

        if (string.IsNullOrEmpty(idToken))
        {
            Debug.LogError(
                "[SESSION HISTORY] Token Firebase manquant."
            );

            if (historyText != null)
                historyText.text =
                    "Impossible de récupérer l'historique.";

            gameObject.SetActive(true);
            return;
        }

        Debug.Log(
            $"[SESSION HISTORY] Récupération de l'historique " +
            $"pour UserID={userID}"
        );

		// --------------------------------------------------------
		// Récupère l'historique du profil sélectionné
		// --------------------------------------------------------
		var sessions =
			await ApiClient.GetSessionsAsync(
				userID,
				idToken
			);
			
		Debug.Log(
			$"[SESSION HISTORY] Nombre de sessions reçues : {sessions?.Length ?? 0}"
		);

		if (sessions != null)
		{
			for (int i = 0; i < sessions.Length; i++)
			{
				Debug.Log(
					$"[SESSION HISTORY] Session {i + 1} -> " +
					$"UserID={sessions[i].UserID}, " +
					$"SessionUid={sessions[i].SessionUid}, " +
					$"Duration={sessions[i].DurationMinutes}, " +
					$"Score={sessions[i].Score}, " +
					$"Errors={sessions[i].Errors}"
				);
			}
		}	

        if (sessions == null || sessions.Length == 0)
        {
            historyText.text = "No sessions found.";
            gameObject.SetActive(true);
            return;
        }

        StringBuilder sb =
            new StringBuilder();

        for (int i = 0; i < sessions.Length; i++)
        {
            var s = sessions[i];

            // ----------------------------------------------------
            // Durée en minutes -> minutes + secondes
            // ----------------------------------------------------
            string durationStr = "N/A";

            if (s.DurationMinutes > 0f)
            {
                int totalSeconds =
                    Mathf.RoundToInt(
                        s.DurationMinutes * 60f
                    );

                int minutes =
                    totalSeconds / 60;

                int seconds =
                    totalSeconds % 60;

                durationStr =
                    $"{minutes:D2}:{seconds:D2}";
            }

			sb.AppendLine(
				$"Session {i + 1}"
			);

			sb.AppendLine(
				$"Durée : {durationStr}"
			);

			sb.AppendLine(
        		$"Score : {s.Score}"
    		);

    		sb.AppendLine(
        		$"Erreurs : {s.Errors}"
    		);

    		sb.AppendLine();
        }

        historyText.text =
            sb.ToString();

        gameObject.SetActive(true);
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }
}