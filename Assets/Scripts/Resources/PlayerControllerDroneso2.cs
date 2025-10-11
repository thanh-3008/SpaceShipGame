using UnityEngine;

public class PlayerControllerDroneso2 : MonoBehaviour
{
    void Start()
    {
        // Spawn 2 drone quay quanh 2 phía
        SpawnDrones();
    }

    void SpawnDrones()
    {
        //leftDrone = Instantiate(dronePrefab, transform.position + new Vector3(-1.5f, 0, 0), Quaternion.identity);
        //rightDrone = Instantiate(dronePrefab, transform.position + new Vector3(1.5f, 0, 0), Quaternion.identity);

        //DroneController leftCtrl = leftDrone.GetComponent<DroneController>();
        //DroneController rightCtrl = rightDrone.GetComponent<DroneController>();

        //leftCtrl.player = transform;
        //rightCtrl.player = transform;
    }
}
