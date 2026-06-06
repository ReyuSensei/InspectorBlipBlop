using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIFadeController : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Transform panel;
    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private float waitBetween = 1f;
    [SerializeField] private float typeSpeed = 0.05f;
    [SerializeField] private string mainMenuScene = "MainMenu";
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip typeSound;
    [SerializeField] private float pitchMin = 0.9f;
    [SerializeField] private float pitchMax = 1.1f;

    void Start()
    {
        StartCoroutine(FadeChildrenInOrder(panel, fadeDuration, waitBetween));
    }

    IEnumerator FadeImage(Image img, float targetAlpha, float duration)
    {
        float startAlpha = img.color.a;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            img.color = new Color(img.color.r, img.color.g, img.color.b,
                            Mathf.Lerp(startAlpha, targetAlpha, elapsed / duration));
            yield return null;
        }

        img.color = new Color(img.color.r, img.color.g, img.color.b, targetAlpha);
    }

    IEnumerator FadeChildrenInOrder(Transform panel, float duration, float waitBetween)
    {
        TMP_Text[] texts = panel.GetComponentsInChildren<TMP_Text>();

        foreach (TMP_Text text in texts)
        {
            yield return StartCoroutine(TypeText(text));
            yield return new WaitForSeconds(waitBetween);
            yield return StartCoroutine(FadeTMP(text, 0f, duration));
        }

        SceneManager.LoadScene(mainMenuScene);
    }

    IEnumerator TypeText(TMP_Text text)
    {
        string fullText = text.text;
        text.text = "";

        text.color = new Color(text.color.r, text.color.g, text.color.b, 1f);

        foreach (char c in fullText)
        {
            text.text += c;
            PlayTypeSound();
            yield return new WaitForSeconds(typeSpeed);
        }
    }

    void PlayTypeSound()
    {
        if (audioSource == null || typeSound == null) return;
        audioSource.pitch = Random.Range(pitchMin, pitchMax);
        audioSource.PlayOneShot(typeSound);
    }

    IEnumerator FadeTMP(TMP_Text text, float targetAlpha, float duration)
    {
        float startAlpha = text.color.a;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            text.color = new Color(text.color.r, text.color.g, text.color.b,
                            Mathf.Lerp(startAlpha, targetAlpha, elapsed / duration));
            yield return null;
        }

        text.color = new Color(text.color.r, text.color.g, text.color.b, targetAlpha);
    }

    public void StartImageFadeIn() => StartCoroutine(FadeImage(image, 1f, fadeDuration));
    public void StartImageFadeOut() => StartCoroutine(FadeImage(image, 0f, fadeDuration));
    public void StartTextSequence() => StartCoroutine(FadeChildrenInOrder(panel, fadeDuration, waitBetween));
}