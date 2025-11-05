using UnityEngine;

public class AudioManagement : MonoBehaviour
{
    public AudioSource backgroundMusic;
    public AudioSource soundEffect;
    public AudioSource sfxto;
    public AudioSource sfxmove;
    public AudioClip newBackgroundMusic;
    public AudioClip thienthachno;
    public AudioClip tiengdan;
    public AudioClip tiengtenlua;
    public AudioClip tiengancoin;
    public AudioClip tiengvacham;
    public AudioClip tiengdanpro;
    public AudioClip tiengcanhbao;
    public AudioClip amthanhtuluc;
    public AudioClip amthanhbatkimcangbathoai;
    public AudioClip amthanhbiendoi;
    public AudioClip amthanhngungdong;
    public AudioClip amthanhlaserden;
    public AudioClip amthanhSkill;
    public AudioClip amthanhfireball;
    public AudioClip vachamfireball;
    public AudioClip amthanhmattroi;
    public AudioClip amthanhdanez;
    public AudioClip bossSpawn;
    public AudioClip bossDashskill;
    public AudioClip musicBoss;
    public AudioClip blockShield;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        backgroundMusic.clip = newBackgroundMusic;
        backgroundMusic.Play();
    }

    public void PlaySfx(AudioClip sfxClip)
    {
        soundEffect.clip = sfxClip;
        soundEffect.PlayOneShot(sfxClip);
    }
    public void PlaySfxto(AudioClip sfxClip)
    {
        sfxto.clip = sfxClip;
        sfxto.PlayOneShot(sfxClip);
    }
    public void Playdichuyen(AudioClip sfxClip)
    {
        sfxmove.clip = sfxClip;
        sfxmove.PlayOneShot(sfxClip);
    }


}
