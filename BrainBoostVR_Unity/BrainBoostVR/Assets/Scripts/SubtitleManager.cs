using UnityEngine;
using TMPro;
using System.Collections;

public class SubtitleManager : MonoBehaviour
{
    public TextMeshProUGUI subtitleText; // assigner le SubtitleText
    public AudioSource audioSource;      // assigner l'AudioSource des consignes

    [System.Serializable]
    public struct Subtitle
    {
        public float time;
        public string text;
    }

    public Subtitle[] subtitles;

    public float fadeDuration = 0.3f; // durée du fade in/out

    private void Start()
    {
        if(audioSource != null && subtitles.Length > 0)
            StartCoroutine(PlaySubtitles());
    }

    private IEnumerator PlaySubtitles()
    {
        subtitleText.text = "";
        foreach(var sub in subtitles)
        {
            // Attendre jusqu'au moment du sous-titre
            while(audioSource.time < sub.time)
                yield return null;

            // Afficher le texte avec fade
            StartCoroutine(FadeIn(sub.text));
        }

        // Effacer le texte à la fin
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeIn(string text)
    {
        subtitleText.text = text;
        float t = 0f;
        while(t < fadeDuration)
        {
            t += Time.deltaTime;
            subtitleText.alpha = Mathf.Lerp(0, 1, t/fadeDuration);
            yield return null;
        }
        subtitleText.alpha = 1;
    }

    private IEnumerator FadeOut()
    {
        float t = 0f;
        while(t < fadeDuration)
        {
            t += Time.deltaTime;
            subtitleText.alpha = Mathf.Lerp(1, 0, t/fadeDuration);
            yield return null;
        }
        subtitleText.alpha = 0;
    }
}
