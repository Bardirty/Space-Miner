using System.Collections;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    [SerializeField] private float spawnTime;
    [SerializeField] private float lifeTime;
    [SerializeField] private GameObject[] prefabs;
    [SerializeField] private int countOfObjects;
    [SerializeField] private float radius;
    private int currentCountOfObjects = 0;
    
    void Start()
    {
        StartCoroutine(Spawner());
    }

    private IEnumerator Spawner()
    {
        yield return new WaitForSeconds(spawnTime);
        if(currentCountOfObjects < countOfObjects)
        {
            GameObject newObj = prefabs[Random.Range(0, prefabs.Length)];
            Instantiate(newObj, new Vector3(Random.Range(0, radius), Random.Range(0, radius), Random.Range(0, radius)), newObj.transform.rotation);
            currentCountOfObjects++;
            Invoke(nameof(DestroyObject),lifeTime);
        }
        StartCoroutine(Spawner());
    }

    private void DestroyObject(GameObject obj)
    {
        Destroy(obj.gameObject);
    }
}
