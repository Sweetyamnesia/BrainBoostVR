using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class FinalGamePanel : MonoBehaviour
{
    public TextMeshProUGUI textScore;
    public TextMeshProUGUI textObjetsTrouves;
    public TextMeshProUGUI textObjetsManquants;
	public TextMeshProUGUI textTemps;

	public AudioSource audioSource;
	public AudioClip clickSound;


    public void DisplayEnd(int score, int objetsTrouves, int objetsManquants, float temps)
    {
        textScore.text = "Score final: " + score;
        textObjetsTrouves.text = "Objets trouvés: " + objetsTrouves;
        textObjetsManquants.text = "Objets manquants: " + objetsManquants;

        int minutes = Mathf.FloorToInt(temps / 60);
        int secondes = Mathf.FloorToInt(temps % 60);
        textTemps.text = $"Temps passé: {minutes:00}:{secondes:00}";

        gameObject.SetActive(true);
    }

    public void Restart()
	{
        PlayClick();
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Menu()
	{
        PlayClick();
		SceneManager.LoadScene("MenuPrincipal"); // Nom de ta scène menu
    }

	public void Quit()
	{
		PlayClick();
		Application.Quit();
	}
	
	private void PlayClick()
	{
		if (audioSource && clickSound)
			audioSource.PlayOneShot(clickSound);
	}
}
