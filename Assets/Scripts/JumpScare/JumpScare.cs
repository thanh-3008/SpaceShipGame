using UnityEngine;

public class JumpScare : MonoBehaviour
{
    public AudioSource jumpScareAudio;
    public AudioClip jumpScare;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        jumpScareAudio.clip = jumpScare;
        jumpScareAudio.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
