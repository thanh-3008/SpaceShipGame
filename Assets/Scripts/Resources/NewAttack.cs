using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewAttack : MonoBehaviour
{
    public GameObject PlayerProjectilePrefab;
    public Transform firePoint;
    public int poolSize = 20;
    public float PlayerProjectileSpeed = 15f;
    public float PlayerProjectileLifetime = 4f;

    public float fireRate = 0.2f;
    public bool allowHoldToFire = true;
    public KeyCode fireKey = KeyCode.Space;
    public string fireButton = "Fire1";

    public ParticleSystem muzzleFlash;
    public AudioSource shootAudio;

    private List<GameObject> pool;
    private float nextFireTime = 0f;

    void Start()
    {
        if (PlayerProjectilePrefab == null)
        {
            Debug.LogError("PlayerProjectilePrefab is null");
            enabled = false;
            return;
        }

        if (firePoint == null)
        {
            GameObject go = new GameObject("FirePoint");
            go.transform.SetParent(transform);
            go.transform.localPosition = Vector3.up * 0.6f;
            firePoint = go.transform;
        }

        pool = new List<GameObject>(poolSize);
        for (int i = 0; i < poolSize; i++)
        {
            GameObject p = Instantiate(PlayerProjectilePrefab);
            p.SetActive(false);
            pool.Add(p);
        }
    }

    void Update()
    {
        bool fireInput = false;

        if (allowHoldToFire)
            fireInput = Input.GetKey(fireKey) || Input.GetButton(fireButton);
        else
            fireInput = Input.GetKeyDown(fireKey) || Input.GetButtonDown(fireButton);

        if (fireInput && Time.time >= nextFireTime)
        {
            Fire();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Fire()
    {
        GameObject proj = GetPooledPlayerProjectile();
        if (proj == null) proj = Instantiate(PlayerProjectilePrefab);

        proj.transform.position = firePoint.position;
        proj.transform.rotation = firePoint.rotation;
        proj.SetActive(true);

        Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 dir = firePoint.up;
            rb.linearVelocity = dir * PlayerProjectileSpeed;
        }
        else
        {
            PlayerProjectile pComp = proj.GetComponent<PlayerProjectile>();
            if (pComp != null) pComp.Launch(firePoint.up * PlayerProjectileSpeed);
        }

        PlayerProjectile projComp = proj.GetComponent<PlayerProjectile>();
        if (projComp != null)
        {
            projComp.SetAutoDisableTime(PlayerProjectileLifetime);
        }
        else
        {
            StartCoroutine(DisableAfterSeconds(proj, PlayerProjectileLifetime));
        }

        if (muzzleFlash != null) muzzleFlash.Play();
        if (shootAudio != null) shootAudio.Play();
    }

    private GameObject GetPooledPlayerProjectile()
    {
        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].activeInHierarchy)
                return pool[i];
        }
        return null;
    }

    private IEnumerator DisableAfterSeconds(GameObject obj, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        if (obj != null) obj.SetActive(false);
    }
}
