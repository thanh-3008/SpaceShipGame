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

    [Header("Audio Sources")]
    public AudioSource sfxSource;
    public AudioSource musicSource; // <-- FIX 1: Thêm biến musicSource bị thiếu

    [Header("Sound Effects List")]
    public List<SoundEffect> soundEffects;

    private Dictionary<string, SoundEffect> soundEffectDictionary;

    // --- FIX 1: Thêm các biến bị thiếu ---
    [Header("Volume Control")]
    [Range(0f, 1f)] private float masterVolume = 1f;
    [Range(0f, 1f)] private float musicVolume = 1f;
    [Range(0f, 1f)] private float sfxVolume = 1f;

    [Header("SFX Object Pooling")]
    [Tooltip("Số lượng AudioSource tạo sẵn để phát SFX")]
    public int sfxPoolSize = 10;
    private List<AudioSource> sfxSourcePool;
    // ------------------------------------

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

        // (Giả sử musicSource cũng cần được thêm nếu thiếu)
        if (musicSource == null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.playOnAwake = false;
            musicSource.loop = true; // Nhạc nền thường lặp lại
        }


        // Tạo dictionary để tra nhanh tên sound
        soundEffectDictionary = new Dictionary<string, SoundEffect>();
        foreach (var sfx in soundEffects)
        {
            // FIX 2: Sửa lỗi gõ sai tên (sfxDictionary -> soundEffectDictionary)
            if (!string.IsNullOrEmpty(sfx.name) && !soundEffectDictionary.ContainsKey(sfx.name))
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

            // LƯU Ý: Code này đang dùng sfxSource chính, không dùng pool.
            // Cân nhắc dùng GetAvailableSfxSource() để phát nhiều âm thanh cùng lúc.
            sfxSource.pitch = sfx.pitch;
            sfxSource.loop = sfx.loop;

            // Áp dụng volume tổng
            sfxSource.PlayOneShot(sfx.clip, sfx.volume * sfxVolume * masterVolume);
        }
        else
        {
            Debug.LogWarning("SoundEffectManager: Không tìm thấy SFX với tên: " + name);
        }
    }


    public void SetMasterVolume(float volume) { masterVolume = Mathf.Clamp01(volume); UpdateAllVolumes(); }
    public void SetMusicVolume(float volume) { musicVolume = Mathf.Clamp01(volume); UpdateAllVolumes(); }
    public void SetSfxVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        // LƯU Ý: Cần cập nhật cả volume của sfxSource và các source trong pool ở đây
        sfxSource.volume = sfxVolume * masterVolume;
    }

    private void UpdateAllVolumes()
    {
        musicSource.volume = masterVolume * musicVolume;
        sfxSource.volume = sfxVolume * masterVolume;
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
        return null; // Trả về null nếu hết pool
    }


    // FIX 3: Chỉ định rõ System.Collections.IEnumerator cho Coroutine
    private System.Collections.IEnumerator FadeMusic(AudioClip newClip, float duration)
    {
        yield return StartCoroutine(FadeOutMusic(duration / 2));

        musicSource.clip = newClip;
        musicSource.Play();

        yield return StartCoroutine(FadeInMusic(duration / 2));
    }

    // FIX 3: Chỉ định rõ System.Collections.IEnumerator cho Coroutine
    private System.Collections.IEnumerator FadeOutMusic(float duration)
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

    // FIX 3: Chỉ định rõ System.Collections.IEnumerator cho Coroutine
    private System.Collections.IEnumerator FadeInMusic(float duration)
    {
        float targetVolume = masterVolume * musicVolume;
        float timer = 0f;
        musicSource.volume = 0; // Bắt đầu từ 0
        musicSource.Play(); // Đảm bảo nó đang Play (nếu bị Stop ở FadeOut)

        while (timer < duration)
        {
            timer += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(0, targetVolume, timer / duration);
            yield return null;
        }

        musicSource.volume = targetVolume;
    }

    /// <summary>
    /// Dừng tất cả âm thanh hiệu ứng (SFX)
    /// </summary>
    public void StopAll()
    {
        sfxSource.Stop();
        // Bạn cũng nên dừng tất cả các source trong pool
        foreach (var source in sfxSourcePool)
        {
            source.Stop();
        }
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
        sfxSource.PlayOneShot(sfx.clip, sfx.volume * sfxVolume * masterVolume);
    }

    /// <summary>
    /// Làm mờ âm thanh hiện tại dần dần (fade out)
    /// </summary>
    public void FadeOut(float duration = 1f)
    {
        StartCoroutine(FadeOutCoroutine(duration));
    }

    // Hàm này đã đúng (dùng System.Collections.IEnumerator)
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