using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public struct SoundEffect
{
    [Tooltip("Tên để gọi âm thanh từ code, ví dụ: 'Player_Shoot'")]
    public string name;

    [Tooltip("File âm thanh")]
    public AudioClip clip;
    [Range(0f, 1f)] public float volume;
    [Range(0.5f, 1.5f)] public float pitch;
    public bool loop;
}

public class SoundEffectManager : MonoBehaviour
{
    public static SoundEffectManager instance;

    [Header("Audio Source")]
    public AudioSource sfxSource;

    [Header("Sound Effects List")]
    public List<SoundEffect> soundEffects;

    private Dictionary<string, SoundEffect> soundEffectDictionary;

    void Awake()
    {
        // Đảm bảo singleton
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

        // Nếu chưa có AudioSource thì tự thêm
        if (sfxSource == null)
        {
            sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSource.playOnAwake = false;
        }

        // Tạo dictionary để tra nhanh tên sound
        soundEffectDictionary = new Dictionary<string, SoundEffect>();
        foreach (var sfx in soundEffects)
        {
            if (!string.IsNullOrEmpty(sfx.name) && !sfxDictionary.ContainsKey(sfx.name))
            {
                soundEffectDictionary.Add(sfx.name, sfx);
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

    /// <summary>
    /// Phát âm thanh theo tên
    /// </summary>
    public void Play(string name)
    {
        if (soundEffectDictionary.TryGetValue(name, out SoundEffect sfx))
        {
            if (sfx.clip == null)
            {
                Debug.LogWarning($"SoundEffectManager: Clip for '{name}' is missing!");
                return;
            }

            sfxSource.pitch = sfx.pitch;
            sfxSource.loop = sfx.loop;
            sfxSource.PlayOneShot(sfx.clip, sfx.volume);
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

    /// <summary>
    /// Dừng tất cả âm thanh hiện tại
    /// </summary>
    public void StopAll()
    {
        sfxSource.Stop();
    }

    /// <summary>
    /// Phát ngẫu nhiên 1 sound trong danh sách
    /// </summary>
    public void PlayRandom()
    {
        if (soundEffects.Count == 0) return;

        int randomIndex = Random.Range(0, soundEffects.Count);
        var sfx = soundEffects[randomIndex];

        if (sfx.clip == null)
        {
            Debug.LogWarning($"SoundEffectManager: Random sound missing clip at index {randomIndex}");
            return;
        }

        sfxSource.pitch = sfx.pitch;
        sfxSource.loop = sfx.loop;
        sfxSource.PlayOneShot(sfx.clip, sfx.volume);
    }

    /// <summary>
    /// Làm mờ âm thanh hiện tại dần dần (fade out)
    /// </summary>
    public void FadeOut(float duration = 1f)
    {
        StartCoroutine(FadeOutCoroutine(duration));
    }

    private System.Collections.IEnumerator FadeOutCoroutine(float duration)
    {
        float startVolume = sfxSource.volume;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            sfxSource.volume = Mathf.Lerp(startVolume, 0f, t / duration);
            yield return null;
        }

        sfxSource.Stop();
        sfxSource.volume = startVolume;
    }
}
