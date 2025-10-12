using UnityEngine;

public class PlayerControllerDroneso2 : MonoBehaviour
{
    private GameObject dronePrefab;
    private Transform leftSpawn;
    private Transform rightSpawn;

    private bool hasDroneItem = false; // chỉ có sau khi nhặt
    private bool droneActive = false;

    void Update()
    {
        if (hasDroneItem && Input.GetKeyDown(KeyCode.Q) && !droneActive)
        {
            ActivateDrones();
        }
    }

    private void ActivateDrones()
    {
        Instantiate(dronePrefab, leftSpawn.position, Quaternion.identity);
        Instantiate(dronePrefab, rightSpawn.position, Quaternion.identity);
        droneActive = true;
        Invoke(nameof(ResetDroneSkill), 30f); // hết 30s thì có thể dùng lại
    }

    private void ResetDroneSkill()
    {
        droneActive = false;
    }

    void PickupDroneItem()
    {
        hasDroneItem = true;
        Debug.Log("Drone skill unlocked!");
    }
}
