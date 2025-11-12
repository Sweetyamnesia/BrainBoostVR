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
            Debug.LogWarning("[SESSION HISTORY] ScoreManager not assigned!");
        }

        string idToken = FirebaseAnonymousAuth.IdToken;
		string firebaseUID = FirebaseAnonymousAuth.UserId;

		// Récupère l'historique depuis l'API
		//var sessions = await ApiClient.GetSessionsAsync(firebaseUID, idToken);
		var sessions = new ApiClient.UnitySessionDto[0];
		

        if (sessions == null || sessions.Length == 0)
        {
            historyText.text = "No sessions found.";
            gameObject.SetActive(true);
            return;
        }

        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < sessions.Length; i++)
        {
            var s = sessions[i];

            // Durée en minutes -> minutes + secondes pour plus de lisibilité
            string durationStr = "N/A";
            if (s.DurationMinutes > 0f)
            {
                int totalSeconds = Mathf.RoundToInt(s.DurationMinutes * 60f);
                int minutes = totalSeconds / 60;
                int seconds = totalSeconds % 60;
                durationStr = $"{minutes:D2}:{seconds:D2}";
            }

            sb.AppendLine($"Session {i + 1}: Duration {durationStr}");
        }

        historyText.text = sb.ToString();
        gameObject.SetActive(true);
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }
}
