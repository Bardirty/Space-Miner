using UnityEngine;

public class LevelController : MonoBehaviour
{
    public static int difficulty = 1;
    [SerializeField] private GameObject[] asteroidPrefabs;
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private Transform asteroids;
    [SerializeField] private Transform enemies;
    [SerializeField, Range(0, 1000)] private float spawnRange;
    [SerializeField, Range(1, 100)] private int minAmountOfAsteroids;
    [SerializeField, Range(10, 1000)] private int maxAmountOfAsteroids;
    [SerializeField, Range(1, 100)] private int minAmountOfEnemies;
    [SerializeField, Range(10, 1000)] private int maxAmountOfEnemies;

    private void Start()
    {
        for (int i = 0; i < Random.Range(minAmountOfAsteroids, maxAmountOfAsteroids) * difficulty; i++)
            GenerateObject(asteroidPrefabs[Random.Range(0, asteroidPrefabs.Length)], asteroids);
        for (int i = 0; i < Random.Range(minAmountOfEnemies, maxAmountOfEnemies) * difficulty; i++)
            GenerateObject(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], enemies);
    }

    private void GenerateObject(GameObject prefab, Transform parent)
    {
        Instantiate(prefab, Random.onUnitSphere * spawnRange, Random.rotation, parent);
    }
}
