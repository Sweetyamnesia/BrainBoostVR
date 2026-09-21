using UnityEngine;
using System.Collections;

public class AudioFade : MonoBehaviour
{
    public AudioSource audioSource;
    public float fadeDuration = 2f;

    public void FadeIn()
    {
        StartCoroutine(Fade(0f, 1f));
    }

    public void FadeOut()
    {
        StartCoroutine(Fade(1f, 0f));
    }

    private IEnumerator Fade(float startVol, float endVol)
    {
        float t = 0f;
        audioSource.volume = startVol;
        audioSource.Play();
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVol, endVol, t / fadeDuration);
            yield return null;
        }
        audioSource.volume = endVol;
        if (endVol == 0f)
            audioSource.Stop();
    }
}
