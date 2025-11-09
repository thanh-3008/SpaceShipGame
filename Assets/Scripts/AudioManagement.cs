using UnityEngine;

public class AudioManagement : MonoBehaviour
{
    public AudioSource backgroundMusic;
    public AudioSource soundEffect;
    public AudioSource sfxto;
    public AudioSource sfxmove;
    public AudioClip newBackgroundMusic; // Nhạc nền mặc định
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
    public AudioClip musicBoss; // Nhạc boss
    public AudioClip blockShield;

    void Start()
    {
        // Chạy nhạc nền mặc định lúc bắt đầu
        if (newBackgroundMusic != null)
        {
            backgroundMusic.clip = newBackgroundMusic;
            backgroundMusic.loop = true; // <--- MỚI: Đảm bảo nhạc nền lặp lại
            backgroundMusic.Play();
        }
    }

    // --- HÀM MỚI ĐỂ CHUYỂN NHẠC BOSS ---
    public void PlayBossMusic()
    {
        if (musicBoss == null) return; // Không có nhạc boss thì thôi

        // Chỉ chuyển nhạc nếu nhạc đang phát KHÔNG phải là nhạc boss
        if (backgroundMusic.clip != musicBoss)
        {
            backgroundMusic.Stop();
            backgroundMusic.clip = musicBoss;
            backgroundMusic.loop = true;
            backgroundMusic.Play();
        }
    }

    // --- HÀM MỚI ĐỂ QUAY LẠI NHẠC CŨ ---
    public void PlayDefaultMusic()
    {
        if (newBackgroundMusic == null) return; // Không có nhạc nền thì thôi

        // Chỉ chuyển nhạc nếu nhạc đang phát KHÔNG phải là nhạc nền
        if (backgroundMusic.clip != newBackgroundMusic)
        {
            backgroundMusic.Stop();
            backgroundMusic.clip = newBackgroundMusic;
            backgroundMusic.loop = true;
            backgroundMusic.Play();
        }
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