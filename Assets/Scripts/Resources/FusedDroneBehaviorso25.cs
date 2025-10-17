using UnityEngine;

public class FusedDroneBehaviorso25 : MonoBehaviour
{
    [SerializeField]
    private float rotationSpeed = 50f;
    [SerializeField]
    private float fireRate = 0.2f;
    private GameObject bulletPrefab;
    private Transform firePoint;

    private float fireTimer;

    void Update()
    {
        transform.RotateAround(Vector3.zero, Vector3.forward, rotationSpeed * Time.deltaTime);

        fireTimer += Time.deltaTime;
        if (fireTimer >= fireRate)
        {
            Fire();
            fireTimer = 0;
        }
    }

    void Fire()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
}
