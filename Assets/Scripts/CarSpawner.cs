using UnityEngine;
using UnityEngine.UI; // For UI Button

public class CarSpawner : MonoBehaviour
{
    [Header("Car Prefab")]
    public GameObject carPrefab; // Assign your Car prefab (with CarAI component)

    [Header("Spawn Points")]
    public Transform[] spawnPoints; // Assign multiple spawn point Transforms in the Inspector

    [Header("Button")]
    public Button spawnButton; // Assign your UI Button here

    private void Start()
    {
        if (spawnButton != null)
        {
            spawnButton.onClick.AddListener(SpawnRandomCar);
        }
        else
        {
            Debug.LogWarning("Spawn Button not assigned!");
        }

        // Optional: Spawn one car automatically at start
        // SpawnRandomCar();
    }

    public void SpawnRandomCar()
    {
        if (carPrefab == null)
        {
            Debug.LogError("Car Prefab is not assigned!");
            return;
        }

        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("No spawn points assigned!");
            return;
        }

        // Pick a random spawn point
        int randomIndex = Random.Range(0, spawnPoints.Length);
        Transform chosenSpawn = spawnPoints[randomIndex];

        // Spawn the car
        GameObject newCar = Instantiate(carPrefab, chosenSpawn.position, chosenSpawn.rotation);

        Debug.Log($"Car spawned at {chosenSpawn.name}");
    }
}