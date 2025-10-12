using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public struct SoundEffect
{
    public string name;
    public AudioClip clip;
}

public class SoundEffectManager : MonoBehaviour
{
    public static SoundEffectManager instance;
    public AudioSource sfxSource;
    public List<SoundEffect> soundEffects;
    private Dictionary<string, AudioClip> soundEffectDictionary;

    void Awake()
    {
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

        if (sfxSource == null)
        {
            sfxSource = gameObject.AddComponent<AudioSource>();
        }

        soundEffectDictionary = new Dictionary<string, AudioClip>();
        foreach (var sfx in soundEffects)
        {
            if (!soundEffectDictionary.ContainsKey(sfx.name))
            {
                soundEffectDictionary.Add(sfx.name, sfx.clip);
            }
        }
    }

    public void Play(string name)
    {
        if (soundEffectDictionary.TryGetValue(name, out AudioClip clip))
        {
            sfxSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("SoundEffectManager: Sound not found: " + name);
        }
    }
}