using UnityEngine;

public class DroneFusionControllerSo25 : MonoBehaviour
{
    private GameObject dronePrefab;
    private GameObject fusedDronePrefab;
    [SerializeField]
    private float fusionRange = 2f;

    [Header("Fusion Effects")]
    private GameObject fusionEffectPrefab;     // Hiệu ứng ánh sáng xoáy
    private AudioClip fusionSound;             // Âm thanh Fusion!
    private AudioSource audioSource;

    private GameObject[] activeDrones;
    private bool isFused = false;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
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

                // Hiệu ứng hợp thể
                if (fusionEffectPrefab != null)
                    Instantiate(fusionEffectPrefab, fusionPos, Quaternion.identity);

                // Âm thanh hợp thể
                if (fusionSound != null)
                    audioSource.PlayOneShot(fusionSound);

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
