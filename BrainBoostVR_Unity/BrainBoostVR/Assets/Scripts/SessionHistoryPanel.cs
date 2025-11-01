using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Text;

public class SessionHistoryPanel : MonoBehaviour
{
    [Header("Références UI")]
    public Transform contentParent;             // Conteneur (Content du ScrollView)
    public GameObject sessionEntryPrefab;       // Prefab d'une ligne (Score / Temps / Objets)
    public Button closeButton;                  // Bouton "Fermer"

	[Header("Références de données")]
	public ScoreManager scoreManager;           // Assigné dans l'inspecteur

    public GameObject finalGamePanel; 
	private void Start()
    {
        if (closeButton != null)
            closeButton.onClick.AddListener(ClosePanel);

        gameObject.SetActive(false);
    }

    public void OpenPanel()
    {
        if (scoreManager == null)
        {
            Debug.LogWarning("[SESSION HISTORY] Aucun ScoreManager assigné !");
            return;
        }

        // Nettoie le contenu précédent
        foreach (Transform child in contentParent)
            Destroy(child.gameObject);

        // Affiche les anciennes sessions
        foreach (var session in scoreManager.sessionHistory)
        {
            GameObject entry = Instantiate(sessionEntryPrefab, contentParent);
            TextMeshProUGUI text = entry.GetComponentInChildren<TextMeshProUGUI>();

            if (text != null)
            {
                string timeFormatted = $"{Mathf.FloorToInt(session.timeSpent / 60):00}:{Mathf.FloorToInt(session.timeSpent % 60):00}";
                text.text = $"Score: {session.score}/{scoreManager.maxScore} | Objets: {session.objectsPlaced} | Temps: {timeFormatted}";
            }
        }

        gameObject.SetActive(true);
    }

    public void ClosePanel()
    {
		gameObject.SetActive(false);

		if (finalGamePanel != null)
        	finalGamePanel.SetActive(true);
    }
}
