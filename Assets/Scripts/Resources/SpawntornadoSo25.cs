using UnityEngine;

public class SpawntornadoSo25 : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Random.Range(0, 2000) < 2) // tỉ lệ thấp
        {
            Vector2 pos = new Vector2(Random.Range(-5f, 5f), Random.Range(-3f, 3f));
            //Instantiate(tornadoPrefab, pos, Quaternion.identity);
        }
    }
}
