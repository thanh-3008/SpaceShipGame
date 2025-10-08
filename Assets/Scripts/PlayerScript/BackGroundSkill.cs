using System.Collections;
using UnityEngine;

public class BackGroundSKill : MonoBehaviour
{
    public SpriteRenderer backgroundRenderer; // Gán trong Inspector
    public Sprite newBackground; // Hình mới (PNG dải băng xanh + tàu)
    public Sprite backGroundDefault;
    public AudioManagement audioManagement;
    public PlayerController playerController;
    public GameObject tiaSang;
    public void Start()
    {
        GameObject audioObj = GameObject.Find("AudioManagement");
        audioManagement = audioObj.GetComponent<AudioManagement>();
        GameObject backgroundobj = GameObject.Find("BackGroundSkillNo");
        backgroundRenderer = backgroundobj.GetComponent<SpriteRenderer>();  
    }
    void Update()
    {
        
    }
    public void StartSkill()
    {
        StartCoroutine(DoiBackGround());
    }
    public IEnumerator DoiBackGround()
    {
        backgroundRenderer.enabled = true;
        tiaSang.SetActive(true);
        audioManagement.PlaySfxto(audioManagement.amthanhSkill);    
        Time.timeScale = 0f;
        backgroundRenderer.sprite = newBackground;
        yield return new WaitForSecondsRealtime(1);
        tiaSang.SetActive(false);
        Time.timeScale = 1f;
        backgroundRenderer.enabled = false;
        backgroundRenderer.sprite = backGroundDefault;

    }
}
