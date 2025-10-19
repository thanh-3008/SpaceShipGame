using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public struct SoundEffect
{
    public string name;
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
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
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
            if (!soundEffectDictionary.ContainsKey(sfx.name))
            {
                soundEffectDictionary.Add(sfx.name, sfx);
            }
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
            Debug.LogWarning("SoundEffectManager: Sound not found: " + name);
        }
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
