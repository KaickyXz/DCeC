using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;

    [Header("Clips")]
    public AudioClip menuMusic;
    public AudioClip minigameMusic;

    private float minigameMusicTime = 0f;
    private bool isPlayingMinigameMusic = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayMenuMusic()
    {
        if (isPlayingMinigameMusic)
        {
            // Salva o tempo antes de trocar
            minigameMusicTime = musicSource.time;
            isPlayingMinigameMusic = false;
        }

        if (musicSource.clip == menuMusic && musicSource.isPlaying)
            return;

        musicSource.clip = menuMusic;
        musicSource.loop = true;
        musicSource.time = 0f;
        musicSource.Play();
    }

    public void PlayMinigameMusic()
    {
        if (isPlayingMinigameMusic && musicSource.isPlaying)
            return;

        musicSource.clip = minigameMusic;
        musicSource.loop = true;
        musicSource.time = minigameMusicTime; // Continua de onde parou
        musicSource.Play();
        isPlayingMinigameMusic = true;
    }

    public void ResetMinigameMusic()
    {
        minigameMusicTime = 0f;
        isPlayingMinigameMusic = false;
    }
}