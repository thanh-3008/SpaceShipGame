using UnityEngine;

public class BossSpaceship25 : MonoBehaviour
{
    private GameObject laserPrefab;
    private Transform firePoint;
    private float fireCooldown = 5f;

    private float fireTimer;

    void Update()
    {
        fireTimer += Time.deltaTime;

        if (fireTimer >= fireCooldown)
        {
            FireLaser();
            fireTimer = 0f;
        }
    }

    void FireLaser()
    {
        GameObject laser = Instantiate(laserPrefab, firePoint.position, Quaternion.identity);
        laser.transform.parent = transform; // gắn vào boss nếu muốn laser cố định
        Debug.Log("Boss fired laser!");
    }
}
