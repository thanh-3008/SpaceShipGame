using UnityEngine;

public class LaserController : MonoBehaviour
{
    // Kéo GameObject "LaserHurtbox" vào ô này trong Inspector
    public GameObject laserHurtbox;

    // Hàm này sẽ được gọi bởi Animation Event để bật vùng sát thương
    public void EnableHurtbox()
    {
        if (laserHurtbox != null)
        {
            laserHurtbox.SetActive(true);
        }
    }

    // Hàm này sẽ được gọi bởi Animation Event để tắt vùng sát thương
    public void DisableHurtbox()
    {
        if (laserHurtbox != null)
        {
            laserHurtbox.SetActive(false);
        }
    }
}