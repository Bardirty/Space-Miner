using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class MaxStats
{
    public float health;
    public float damage;
    public float speed;
    public float maxCapacity;

    public int[] goldRequired;
    public int[] platinumRequired;
}

public class ShopController : MonoBehaviour
{
    public MaxStats maxStats;

    [Header("UI")]
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private GameObject interfacePanel;
    [SerializeField] private Slider[] currentValueSliders;
    [SerializeField] private Slider[] potentialValueSliders;
    [SerializeField] private Button[] upgradeButtons;
    [SerializeField] private Slider[] resources;
    [SerializeField] private TextMeshProUGUI[] resourcesText;

    [Header("Evacuation")]
    [SerializeField, Range(0, 10)] private float evacuationSpeed;
    [SerializeField] private PlayerController player;
    [SerializeField] private Transform evacuationPoint;
    [SerializeField] private Transform dropPoint;
    [SerializeField] private Camera shopCamera;
    private void Start()
    {
        player.canControl = true;
    }

    private void FillUI()
    {
        currentValueSliders[0].value = player.stats.health / maxStats.health;
        currentValueSliders[1].value = player.damage / maxStats.damage;
        currentValueSliders[2].value = player.stats.speed / maxStats.speed;
        currentValueSliders[3].value = player.stats.capacity[0] / maxStats.maxCapacity;
        potentialValueSliders[0].value = player.stats.health / maxStats.health + 0.1f;
        potentialValueSliders[1].value = player.damage / maxStats.damage + 0.1f;
        potentialValueSliders[2].value = player.stats.speed / maxStats.speed + 0.1f;
        currentValueSliders[3].value = player.stats.capacity[0] / maxStats.maxCapacity + 0.1f;
        for (int i = 0; i < upgradeButtons.Length; i++)
        {
            upgradeButtons[i].interactable = player.resources.gold >= maxStats.goldRequired[i] && 
                player.resources.platinum >= maxStats.platinumRequired[i];
        }
        resources[0].value = player.resources.gold / player.stats.capacity[0];
        resources[1].value = player.resources.platinum / player.stats.capacity[1];
        resourcesText[0].text = $"{player.resources.gold} / {(int)player.stats.capacity[0]}";
        resourcesText[1].text = $"{player.resources.platinum} / {(int)player.stats.capacity[1]}";
    }

    public void OnDropButtonClick()
    {
        Cursor.visible = false;
        player.transform.position = dropPoint.position;
        shopPanel.SetActive(false);
        interfacePanel.gameObject.SetActive(true);
        player.GetComponent<Rigidbody>().isKinematic = false;
        shopCamera.gameObject.SetActive(false);
        player.mainCam.gameObject.SetActive(true);
        player.canControl = true;
        player.UpdateUI();
    }

    public void OnUpgradeButtonClick(int index)
    {
        if (CheckFull(index) == 1 || player.resources.gold - maxStats.goldRequired[index] < 0 
            || player.resources.platinum - maxStats.platinumRequired[index] < 0) return;
        switch(index)
        {
            case 0:
                player.stats.health += maxStats.health / 10;
                player.health = player.stats.health;
                break;
            case 1:
                player.damage += maxStats.damage / 10;
                break;
            case 2:
                player.stats.speed += maxStats.speed / 10;
                break;
            case 3:
                player.stats.capacity[0] += maxStats.maxCapacity / 10;
                player.stats.capacity[1] += maxStats.maxCapacity / 100;
                break;
        }
        player.resources.gold -= maxStats.goldRequired[index];
        player.resources.platinum -= maxStats.platinumRequired[index];
        player.goldText.text = player.resources.gold.ToString();
        player.platinumText.text = player.resources.platinum.ToString();
        FillUI();
    }
    private int CheckFull(int index)
    {
        switch (index)
        {
            case 0:
                return (player.health == maxStats.health) ? 1 : 0;
            case 1:
                return (player.damage == maxStats.damage) ? 1 : 0;
            case 2:
                return (player.stats.speed == maxStats.speed) ? 1 : 0;
        }
        return 0;
    }

    private void Evacuate()
    {
        Cursor.visible = true;
        FillUI();
        player.GetComponent<Rigidbody>().isKinematic = true;
        shopCamera.gameObject.SetActive(true);
        player.mainCam.gameObject.SetActive(false);
        player.canControl = false;
        float distance = Vector3.Distance(evacuationPoint.position, player.transform.position);
        player.transform.DOMove(evacuationPoint.position, distance * evacuationSpeed);
        player.transform.DORotateQuaternion(evacuationPoint.rotation, distance * evacuationSpeed);
        interfacePanel.gameObject.SetActive(false);
        StartCoroutine(ActivePanel());
    }
    private IEnumerator ActivePanel()
    {
        yield return new WaitForSeconds(5.0f);
        shopPanel.SetActive(true);
    }
    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            Evacuate();
        }
    }
}
