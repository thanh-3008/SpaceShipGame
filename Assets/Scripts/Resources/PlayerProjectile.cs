using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PlayerProjectile : MonoBehaviour
{
    public float damage = 1f;
    public bool useRigidbody = true;
    private float autoDisableTime = 4f;
    private Vector2 velocity;
    private bool launched = false;

    void OnEnable()
    {
        launched = false;
    }

    void Update()
    {
        if (!useRigidbody && launched)
        {
            transform.position += (Vector3)(velocity * Time.deltaTime);
        }
    }

    public void Launch(Vector2 vel)
    {
        velocity = vel;
        launched = true;
    }

    public void SetAutoDisableTime(float seconds)
    {
        autoDisableTime = seconds;
        StopAllCoroutines();
        StartCoroutine(AutoDisableCoroutine());
    }

    private IEnumerator AutoDisableCoroutine()
    {
        yield return new WaitForSeconds(autoDisableTime);
        gameObject.SetActive(false);
    }

}
