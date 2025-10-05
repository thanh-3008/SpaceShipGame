using JetBrains.Annotations;
using System;
using UnityEngine;

public class SpawnSun : MonoBehaviour
{
    public GameObject sunprefap;
    public PlayerController playerController;
    public BackGroundSKill skill;
    public AudioManagement audio;
    private GameObject sun;
    private float khoangcach;
    public AudioSource sfxmove;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject audioobj = GameObject.Find("AudioManagement");
        audio = audioobj.GetComponent<AudioManagement>();
        GameObject audiomove = GameObject.Find("sfxmove");
        sfxmove = audiomove.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && playerController.thanhNoHienTai >= playerController.thanhNoToiDa * 3)
        {
            audio.Playdichuyen(audio.amthanhmattroi);
            skill.StartSkill();
            Vector2 tran = new Vector2(0, -13);
            sun = Instantiate(sunprefap, tran, Quaternion.identity);
            playerController.thanhNoHienTai = 0f;
        }
        if (sun != null)
        {
            float vitritau = playerController.transform.position.y;
            float vitrisun = sun.transform.position.y;
            khoangcach = Math.Abs(vitritau - vitrisun);
            float dolon = 1 - (khoangcach / ((Math.Abs(vitritau) + Math.Abs(vitrisun))));
            sfxmove.volume = dolon;
        }
    }
}