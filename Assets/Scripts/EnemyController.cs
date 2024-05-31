using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float health;
    [SerializeField] private float speed;
    [SerializeField] private float fireRate;
    [SerializeField] private float interactionRange;
    [SerializeField] private float chasingDuration;
    [SerializeField] private float radius;
    [SerializeField] private float maxDistance;
    [SerializeField] private ParticleSystem dieEffect;
    [SerializeField] private AudioSource shootSource;

    [SerializeField] private GameObject healingBall;

    [SerializeField] private ParticleSystem laser;
    public Transform asteroids;
    private Transform target;
    private Vector3 hitTarget;
    private Rigidbody rb;
    private bool isChasing = false;
    private bool inRange = false;
    private bool isShooting = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (!target) SelectTarget();
        transform.LookAt(target.position);

        if (inRange && isShooting) return;

        if (Vector3.Distance(transform.position, target.position) < interactionRange)
        {
            if (!isChasing) SelectTarget();
            else
            {
                isShooting = true;
                Shoot();
                return;
            }
        }

        rb.velocity = transform.forward * speed;
    }

    private void SelectTarget()
    {
        target = (asteroids.childCount > 0) ? asteroids.GetChild(Random.Range(0, asteroids.childCount)) : null;
    }

    private void Shoot()
    {
        if (!isShooting) return;
        if (Physics.SphereCast(transform.position, radius, transform.forward, out RaycastHit hit, maxDistance))
        {
            hitTarget = hit.point;
            shootSource.Play();
            if (hit.transform.TryGetComponent(out AsteroidController asteroid))
            {
                asteroid.TakeDamage(damage);
            }
            else if (hit.transform.TryGetComponent(out PlayerController player))
            {
                player.TakeDamage(damage);
            }
        }
        laser.Play();
        Invoke(nameof(Shoot), 60 / fireRate);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0) Die();
    }

    private void Die()
    {
        Destroy(Instantiate(dieEffect.gameObject, transform.position, transform.rotation), 3.0f);
        Destroy(Instantiate(healingBall.gameObject, transform.position, transform.rotation), 5.0f);
        Destroy(gameObject);
    }

    private IEnumerator Forget()
    {
        if (inRange) yield break;
        yield return new WaitForSeconds(chasingDuration);
        isChasing = false;
        SelectTarget();
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            Debug.Log("Player entered range");
            isChasing = true;
            inRange = true;
            target = col.transform;
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            Debug.Log("Player exited range");
            inRange = false;
            isShooting = false;
            StartCoroutine(Forget());
        }
    }
}
