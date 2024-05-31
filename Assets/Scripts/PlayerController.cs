using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class PlayerStats
{
    public float health;
    public float speed;
    public float angularSpeed;
    public float collectingRange;
    public float collectingForce;

    public float[] capacity;
}

public class PlayerController : MonoBehaviour
{
    public PlayerStats stats;

    [Header("UI")]
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI platinumText;
    public TextMeshProUGUI healthText;
    public Slider healthSlider;
    [SerializeField] private Animator fade;
    [SerializeField] private PauseController pauseMenu;

    [Header("Weapon")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private ParticleSystem[] lasers;
    [SerializeField] private float radius;
    [SerializeField] private float maxDistance;
    [SerializeField] private ParticleSystem dieEffect;
    [SerializeField] private AudioSource soundMaster;
    public float damage;

    public (int gold, int platinum) resources;
    //public (int gold, int platinum) resourcesCapacity;
    private Vector3 target;

    [HideInInspector] public bool canControl;
    [HideInInspector] public Camera mainCam;
    [HideInInspector] public float health;
    [SerializeField] bool invicibility = false;

    private Rigidbody rb;
    private void Start()
    {
        fade.SetTrigger("isUnFaded");

        rb = GetComponent<Rigidbody>();
        mainCam = Camera.main;
        Cursor.visible = false;
        health = stats.health;
        canControl = true;
        //GetInfo();  
        UpdateUI();
    }

    private void Update()
    {
        if (invicibility) health = stats.health;
        if (!canControl) return;
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        rb.velocity += transform.forward * z * stats.speed;

        Vector2 rotationDelta = new Vector3(Screen.width / 2, Screen.height / 2) - Input.mousePosition;
        float rotX = rotationDelta.y * stats.angularSpeed * System.Convert.ToInt32(!pauseMenu.isPaused);
        float rotY = -rotationDelta.x * stats.angularSpeed * System.Convert.ToInt32(!pauseMenu.isPaused);
        transform.Rotate(rotX, rotY, -x);

        if (Input.GetMouseButtonDown(0) && !pauseMenu.isPaused) Shoot();
        if (Input.GetMouseButtonDown(1) && !pauseMenu.isPaused) CollectObjects();
        if (Input.GetKey(KeyCode.LeftControl) && !pauseMenu.isPaused)
            rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, Time.deltaTime * stats.speed);
        if (Input.GetKey(KeyCode.Escape) && !pauseMenu.isPaused) Pause();

    }
    private void Pause()
    {
        Cursor.visible = true;
        pauseMenu.isPaused = true;
        pauseMenu.SwitchPause();
    }

    private void CollectObjects()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, stats.collectingRange);
        foreach (Collider col in colliders)
        {
            Debug.Log(col.name);
            if (!col.CompareTag("Resource")) continue;
            else
            {
                if (col.name.Contains("Gold") && resources.gold < (int)stats.capacity[0])
                    resources.gold++;
                else if (col.name.Contains("Platinum") && resources.platinum < (int)stats.capacity[1])
                    resources.platinum++;
                Destroy(col.gameObject);
                UpdateUI();
            }
            if (col.TryGetComponent(out Rigidbody curRb))
                curRb.velocity = Vector3.Normalize(transform.position - curRb.position) * stats.collectingForce;
        }
    }

    private void Shoot()
    {
        soundMaster.Play();
        Ray ray = mainCam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;
        target = firePoint.position + firePoint.forward * maxDistance;

        if (Physics.SphereCast(ray, radius, out hit, maxDistance))
        {
            target = hit.point;
            if (hit.transform.TryGetComponent(out AsteroidController asteroid))
                asteroid.TakeDamage(damage);
            else if (hit.transform.TryGetComponent(out EnemyController enemy))
                enemy.TakeDamage(damage);

            Debug.DrawLine(ray.origin, hit.point, Color.yellow, 10);
        }

        Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.cyan, 10);
        lasers[Random.Range(0, 2)].Play();
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0) {
            health = 0;
            Die();
        }
        UpdateUI();
    }

    private void Die()
    {
        canControl = false;
        Destroy(Instantiate(dieEffect, transform.position, transform.rotation), 3.0f);
        Destroy(transform.GetChild(1).gameObject);
        Invoke(nameof(ReloadScene), 2f);

    }
    private void ReloadScene()
    {
        SceneManager.LoadScene(1);
    }

    //private void OnTriggerEnter(Collider col)
    //{
    //    switch (col.tag)
    //    {
    //        case "Resource":
    //            if (col.name.Contains("Gold"))
    //                resources.gold++;
    //            else if (col.name.Contains("Platinum"))
    //                resources.platinum++;
    //            Destroy(col.gameObject);
    //            break;
    //    }
    //}

    public void UpdateUI()
    {
        //Resoureces
        goldText.text = $"Gold: {resources.gold}/{(int)stats.capacity[0]}";
        platinumText.text = $"Platinum: {resources.platinum}/{(int)stats.capacity[1]}";
        //Health
        healthText.text = health.ToString();
        healthSlider.value = health / stats.health;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Healing"))
        {
            health += 20;
            if(health > stats.health) health = stats.health;
            Destroy(other.gameObject);
            UpdateUI();
        }
    }
}
