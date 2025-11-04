using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Text;

public class SessionHistoryPanel : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI historyText; // Un seul texte qui affichera toutes les sessions
    public Button closeButton;

    [Header("Data")]
    public ScoreManager scoreManager;

    private void Start()
    {
        if (closeButton != null)
            closeButton.onClick.AddListener(() => ClosePanel());


        gameObject.SetActive(false);
    }

	public async void OpenPanel()
	{
		if (scoreManager == null)
		{
			Debug.LogWarning("[SESSION HISTORY] ScoreManager not assigned!");
			return;
		}

		string idToken = FirebaseAnonymousAuth.IdToken;
		string firebaseUID = FirebaseAnonymousAuth.UserId;

		var sessions = await ApiClient.GetSessionsAsync(firebaseUID, idToken);
		if (sessions == null || sessions.Length == 0)
		{
			historyText.text = "No sessions found.";
			gameObject.SetActive(true);
			return;
		}

		StringBuilder sb = new StringBuilder();
		for (int i = 0; i < scoreManager.sessionHistory.Count; i++)
		{
			var s = scoreManager.sessionHistory[i];
			sb.AppendLine($"Session {i + 1}: Score {s.score}, Errors {s.errors}, Time {s.timeSpent:F1}s");
		}

		historyText.text = sb.ToString();
		gameObject.SetActive(true);
	}
	
	public void ClosePanel()
	{
    	gameObject.SetActive(false);
	}

}
