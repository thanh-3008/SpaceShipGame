using UnityEngine;

// GẮN SCRIPT NÀY VÀO PREFAB KHIÊN BAY (BOOMERANG)
public class Boss4_FlingShield : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDame(50f);
            }
        }
    }
}