using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    public AudioSource lobbyMusic; // Музыка для лобби
    public AudioSource gameMusic;  // Музыка для игры
    public float fadeDuration = 2f; // Длительность перехода между треками
    private AudioSource soundEffect;
    private void Start()
    {
        // Начинаем с музыки лобби
        StartCoroutine(FadeIn(lobbyMusic));
    }

    // Метод для переключения на музыку лобби
    public void SwitchToLobbyMusic()
    {
        StartCoroutine(SwitchMusic(lobbyMusic, gameMusic));
    }

    // Метод для переключения на музыку игры
    public void SwitchToGameMusic()
    {
        StartCoroutine(SwitchMusic(gameMusic, lobbyMusic));
    }

    public void ActivateSoundEffect(AudioSource soundEffect) 

    {   
            if (!soundEffect.isPlaying)
            {
                soundEffect.Play();
            }         
    }

    // Корутина для переключения между треками
    private IEnumerator SwitchMusic(AudioSource musicToFadeIn, AudioSource musicToFadeOut)
    {
        // Плавно уменьшаем громкость текущего трека
        if (musicToFadeOut != null && musicToFadeOut.isPlaying)
        {
            yield return StartCoroutine(FadeOut(musicToFadeOut));
        }

        // Плавно увеличиваем громкость нового трека
        if (musicToFadeIn != null)
        {
            if (!musicToFadeIn.isPlaying)
            {
                musicToFadeIn.Play();
            }
            yield return StartCoroutine(FadeIn(musicToFadeIn));
        }
    }

    // Корутина для плавного увеличения громкости
    private IEnumerator FadeIn(AudioSource audioSource)
    {
        float elapsedTime = 0f;
        float startVolume = 0f;
        float targetVolume = 1f;

        while (elapsedTime < fadeDuration)
        {
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        audioSource.volume = targetVolume; // Убедимся, что громкость точно установлена
    }

    // Корутина для плавного уменьшения громкости
    private IEnumerator FadeOut(AudioSource audioSource)
    {
        float elapsedTime = 0f;
        float startVolume = audioSource.volume;
        float targetVolume = 0f;

        while (elapsedTime < fadeDuration)
        {
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        audioSource.volume = targetVolume; // Убедимся, что громкость точно установлена
        audioSource.Stop(); // Останавливаем воспроизведение
    }
}
