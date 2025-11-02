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
            closeButton.onClick.AddListener(() => gameObject.SetActive(false));

        gameObject.SetActive(false);
    }

	public void OpenPanel()
	{
		if (scoreManager == null)
		{
			Debug.LogWarning("[SESSION HISTORY] ScoreManager not assigned!");
			return;
		}

		StringBuilder sb = new StringBuilder();
		for (int i = 0; i < scoreManager.sessionHistory.Count; i++)
		{
			var s = scoreManager.sessionHistory[i];
			sb.AppendLine($"Session {i + 1}: Score {s.score}, Errors {s.objectsPlaced}, Time {s.timeSpent:F1}s");
		}

		historyText.text = sb.ToString();
		gameObject.SetActive(true);
	}
	
	public void ClosePanel()
	{
    	gameObject.SetActive(false);
	}

}
