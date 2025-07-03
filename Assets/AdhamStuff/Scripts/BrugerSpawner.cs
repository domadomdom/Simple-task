using UnityEngine;

public class BrugerSpawner : MonoBehaviour
{
    [Header("Burger Settings")]
    public GameObject burgerPrefab;
    public Vector3 spawnArea = new Vector3(10, 5, 1);
    public float spawnInterval = 2f;
    public int maxburgers = 10;
    public LayerMask avoidCollisionLayer;

    private float timer = 0f;
    private int currentburgerCount = 0;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval && currentburgerCount < maxburgers)
        {
            Spawnburger();
            timer = 0f;
        }
    }

    void Spawnburger()
    {
        Vector3 spawnPosition = GetRandomSpawnPosition();

        if (avoidCollisionLayer != 0)
        {
            Collider[] colliders = Physics.OverlapSphere(spawnPosition, 0.5f, avoidCollisionLayer);
            if (colliders.Length > 0)
            {
                return;
            }
        }

        Instantiate(burgerPrefab, spawnPosition, Quaternion.identity);
        currentburgerCount++;
    }

    Vector3 GetRandomSpawnPosition()
    {
        float randomX = Random.Range(-spawnArea.x / 2, spawnArea.x / 2);
        float randomY = Random.Range(-spawnArea.y / 2, spawnArea.y / 2);
        float randomZ = Random.Range(-spawnArea.z / 2, spawnArea.z / 2);

        return transform.position + new Vector3(randomX, randomY, randomZ);
    }

    // New Method
    public void OnBurgerDestroyed()
    {
        currentburgerCount--;
    }
}