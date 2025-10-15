using UnityEngine;

public class DroneFusionControllerSo25 : MonoBehaviour
{
    private GameObject dronePrefab;           // Drone gốc
    private GameObject fusedDronePrefab;      // Drone sau khi hợp thể
    private float fusionRange = 2f;           // Khoảng cách tối đa giữa 2 drone để hợp thể
    private GameObject[] activeDrones;
    private bool isFused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) // Nhấn F để hợp thể
        {
            TryFusion();
        }
    }

    void TryFusion()
    {
        activeDrones = GameObject.FindGameObjectsWithTag("Drone");

        if (activeDrones.Length >= 2 && !isFused)
        {
            GameObject droneA = activeDrones[0];
            GameObject droneB = activeDrones[1];
            float distance = Vector3.Distance(droneA.transform.position, droneB.transform.position);

            if (distance <= fusionRange)
            {
                Vector3 fusionPos = (droneA.transform.position + droneB.transform.position) / 2;
                Destroy(droneA);
                Destroy(droneB);
                GameObject fusedDrone = Instantiate(fusedDronePrefab, fusionPos, Quaternion.identity);
                fusedDrone.tag = "Drone";
                isFused = true;
                Debug.Log("⚡ Drone Fusion Success!");
            }
            else
            {
                Debug.Log("❌ Drones too far to fuse.");
            }
        }
    }
}
