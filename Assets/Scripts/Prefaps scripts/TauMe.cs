using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class TauMe : MonoBehaviour
{
    public float speed = 5f;
    private float bodemthoigian;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bodemthoigian = 0f;
        AudioManagement audioscript = FindAnyObjectByType<AudioManagement>();
        audioscript.PlaySfxto(audioscript.tiengcanhbao);       
    }

    // Update is called once per frame
    void Update()
    {
        bodemthoigian += Time.deltaTime;
        if (bodemthoigian <= 13.5f)
        {
            transform.Translate(Vector2.up * speed * Time.deltaTime);
        }
        if (bodemthoigian > 13.5f)
        {
            transform.Translate(Vector2.down * speed * Time.deltaTime);
        }
    }
    private void LateUpdate()
    {
        Vector3 currentPosition = transform.position;
        float clampedY = Mathf.Clamp(currentPosition.y, -15f, -6f);
        transform.position = new Vector3(currentPosition.x, clampedY, currentPosition.z);
        if (currentPosition.y <= -12f)
        {
            Destroy(gameObject);
        }
    }
    
}
