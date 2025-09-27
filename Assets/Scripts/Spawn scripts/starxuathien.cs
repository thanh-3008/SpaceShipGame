using UnityEngine;

public class starxuathien : MonoBehaviour
{
    public GameObject star;
    public float timer = 5f;
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            Instantiate(star, new Vector3(Random.Range(-6f, 6f), transform.position.y, 0f), Quaternion.identity);
            timer = 5f;
        }

    }
}
