using UnityEngine;

using TMPro;

public class spawntenlua : MonoBehaviour

{

    public GameObject tenluaprefap;

    public TextMeshProUGUI soTenLuaText;

    public AudioManagement Audio;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()

    {
        GameObject obj = GameObject.Find("SoTenLua");
        soTenLuaText = obj.GetComponent<TextMeshProUGUI>();
        GameObject objAudio = GameObject.Find("AudioManagement");
        Audio = objAudio.GetComponent<AudioManagement>();
    }



    // Update is called once per frame

    void Update()

    {

        int sotenlua = int.Parse(soTenLuaText.text);

        if (Input.GetKeyDown(KeyCode.E) && sotenlua > 0)

        {

            sotenlua -= 1;
            soTenLuaText.text = sotenlua.ToString();

            Vector2 spawnPosition = new Vector2(transform.position.x, transform.position.y); // Position above the spawner

            AudioManagement audioManagement = Audio.GetComponent<AudioManagement>();
            audioManagement.PlaySfxto(audioManagement.tiengtenlua); // Play the sound effect

            Instantiate(tenluaprefap, spawnPosition, tenluaprefap.transform.rotation); // Spawn the projectile

        }
    }

}