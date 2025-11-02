using UnityEngine;
using TMPro;
using System.Collections;

[System.Serializable]
public struct Subtitle
{
    public float time;   // moment où la phrase doit apparaître
	public string text;  // texte du sous-titre
	public GameObject[] images;
}

public class SubtitleManager : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI subtitleText;
    public CanvasGroup panelGroup; // pour un fade plus propre du panel

    [Header("Config")]
    public Subtitle[] subtitles;
    public float fadeDuration = 0.3f;

    private AudioSource audioSource;

    void Awake()
    {
        // s'assure que le panel est bien caché au lancement
        if (panelGroup != null)
            panelGroup.alpha = 0f;
        else if (subtitleText != null)
            subtitleText.alpha = 0f;
    }

    public void PlaySubtitles(AudioSource source)
    {
        if (subtitles.Length == 0 || subtitleText == null) return;

        StopAllCoroutines();
        StartCoroutine(PlaySequence(source));
    }

    private IEnumerator PlaySequence(AudioSource source)
    {
        // attendre une frame pour éviter le bug du main thread
        yield return null;

        gameObject.SetActive(true);
        audioSource = source;
        subtitleText.text = "";

        if (panelGroup != null)
            StartCoroutine(FadeCanvasGroup(panelGroup, 0f, 1f));

        foreach (var sub in subtitles)
        {
			while (audioSource != null && audioSource.time < sub.time)
				yield return null;

			// Cacher les images avant chaque phrase
			foreach (var s in subtitles)
				if (s.images != null)
					foreach (var img in s.images)
						if (img != null)
							img.SetActive(false);

			StartCoroutine(FadeIn(sub.text));

			// Afficher les images assignées à la phrase
			if (sub.images != null)
				foreach (var img in sub.images)
					if (img != null)
						img.SetActive(true);
        }

        // attendre la fin de l’audio
        if (audioSource != null)
            yield return new WaitWhile(() => audioSource.isPlaying);

		// fade out panel
		if (panelGroup != null)
			yield return StartCoroutine(FadeCanvasGroup(panelGroup, 1f, 0f));
		else
			yield return StartCoroutine(FadeOut());


		foreach (var s in subtitles)
			if (s.images != null)
				foreach (var img in s.images)
					if (img != null)
						img.SetActive(false);
					
        gameObject.SetActive(false);
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup group, float start, float end)
    {
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            group.alpha = Mathf.Lerp(start, end, t / fadeDuration);
            yield return null;
        }
        group.alpha = end;
    }

    private IEnumerator FadeIn(string text)
    {
        subtitleText.text = text;
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            subtitleText.alpha = Mathf.Lerp(0, 1, t / fadeDuration);
            yield return null;
        }
        subtitleText.alpha = 1;
    }

    private IEnumerator FadeOut()
    {
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            subtitleText.alpha = Mathf.Lerp(1, 0, t / fadeDuration);
            yield return null;
        }
        subtitleText.alpha = 0;
    }
}
