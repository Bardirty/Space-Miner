using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidController : MonoBehaviour
{
    [SerializeField] private GameObject[] potentialDrop;
    [SerializeField, Range(0, 100)] private float health;
    

    public void TakeDamage(float damage)
    {
        health -= damage;
        if(health <= 0)
        {
            Explode();
        }

    }

    public void Explode()
    {
        for(int i = 0; i < Random.Range(0, 10);i++)
        {
            Destroy(Instantiate(potentialDrop[Random.Range(0, potentialDrop.Length)], transform.position, Random.rotation).gameObject, 10f);
        }
        Destroy(gameObject);
    }
}
