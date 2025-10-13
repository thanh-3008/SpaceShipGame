
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public struct SoundEffect
{
    [Tooltip("Tên để gọi âm thanh từ code, ví dụ: 'Player_Shoot'")]
    public string name;

    [Tooltip("File âm thanh")]
    public AudioClip clip;
}

public class SoundEffectManager : MonoBehaviour
{
    public static SoundEffectManager instance;

    [Header("Master Audio Control")]
    [Range(0f, 1f)]
    [Tooltip("Âm lượng tổng của game")]
    public float masterVolume = 1.0f;

    [Header("Music Channel")]
    [Range(0f, 1f)]
    [Tooltip("Âm lượng của kênh nhạc nền")]
    public float musicVolume = 0.7f;
    public AudioSource musicSource;

    [Header("SFX Channel")]
    [Range(0f, 1f)]
    [Tooltip("Âm lượng của kênh hiệu ứng âm thanh")]
    public float sfxVolume = 1.0f;

    [Header("SFX Library")]
    [Tooltip("Danh sách tất cả các hiệu ứng âm thanh trong game")]
    public List<SoundEffect> soundEffects;

    private Dictionary<string, AudioClip> sfxDictionary;
    private List<AudioSource> sfxSourcePool;
    private int sfxPoolSize = 15; // Số lượng AudioSource tối đa cho SFX cùng lúc

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Giữ Manager này tồn tại khi chuyển scene
        }
        else
        {
            Destroy(gameObject); // Nếu đã có, hủy bản sao này
            return;
        }

        InitializeManager();
    }

    private void InitializeManager()
    {
        if (musicSource == null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.loop = true; // Nhạc nền thường lặp lại
            musicSource.playOnAwake = false;
        }

        sfxDictionary = new Dictionary<string, AudioClip>();
        foreach (var sfx in soundEffects)
        {
            if (!string.IsNullOrEmpty(sfx.name) && !sfxDictionary.ContainsKey(sfx.name))
            {
                sfxDictionary.Add(sfx.name, sfx.clip);
            }
        }

        sfxSourcePool = new List<AudioSource>();
        for (int i = 0; i < sfxPoolSize; i++)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.playOnAwake = false;
            sfxSourcePool.Add(source);
        }
    }

    public void PlayMusic(AudioClip musicClip, float fadeDuration = 1.0f)
    {
        if (musicClip == null || (musicSource.isPlaying && musicSource.clip == musicClip)) return;

        StartCoroutine(FadeMusic(musicClip, fadeDuration));
    }

    public void StopMusic(float fadeDuration = 1.0f)
    {
        StartCoroutine(FadeOutMusic(fadeDuration));
    }

    public void PlaySFX(string name)
    {
        if (sfxDictionary.TryGetValue(name, out AudioClip clip))
        {
            AudioSource source = GetAvailableSfxSource();
            if (source != null)
            {
                source.clip = clip;
                source.volume = masterVolume * sfxVolume;
                source.Play();
            }
        }
        else
        {
            Debug.LogWarning("SoundEffectManager: Không tìm thấy SFX với tên: " + name);
        }
    }


    public void SetMasterVolume(float volume) { masterVolume = Mathf.Clamp01(volume); UpdateAllVolumes(); }
    public void SetMusicVolume(float volume) { musicVolume = Mathf.Clamp01(volume); UpdateAllVolumes(); }
    public void SetSfxVolume(float volume) { sfxVolume = Mathf.Clamp01(volume); }

    private void UpdateAllVolumes()
    {
        musicSource.volume = masterVolume * musicVolume;
    }


    private AudioSource GetAvailableSfxSource()
    {
        foreach (var source in sfxSourcePool)
        {
            if (!source.isPlaying)
            {
                return source;
            }
        }
        Debug.LogWarning("SoundEffectManager: Hết AudioSource trong pool!");
        return null;
    }


    private IEnumerator FadeMusic(AudioClip newClip, float duration)
    {
        yield return StartCoroutine(FadeOutMusic(duration / 2));

        musicSource.clip = newClip;
        musicSource.Play();

        yield return StartCoroutine(FadeInMusic(duration / 2));
    }

    private IEnumerator FadeOutMusic(float duration)
    {
        float startVolume = musicSource.volume;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(startVolume, 0, timer / duration);
            yield return null;
        }

        musicSource.Stop();
        musicSource.volume = startVolume; // Reset volume for next play
    }

    private IEnumerator FadeInMusic(float duration)
    {
        float targetVolume = masterVolume * musicVolume;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(0, targetVolume, timer / duration);
            yield return null;
        }

        musicSource.volume = targetVolume;
    }
}