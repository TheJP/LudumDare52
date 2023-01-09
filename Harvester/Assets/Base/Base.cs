using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField]
    private float speed = 1f;

    private Vector3 targetPosition;
    private ResourceManager resourceManager;

    private float flyingTime = 0f;

    public void Start()
    {
        targetPosition = transform.position;
        resourceManager = FindObjectOfType<ResourceManager>();
    }

    public void Update()
    {
        var currentFuel = resourceManager.GetStockCounts()[ResourceType.Fuel];
        if (currentFuel <= 0)
        {
            targetPosition = transform.position;
            return;
        }

        var distance = targetPosition - transform.position;
        var step = speed * Time.deltaTime;
        if (distance.sqrMagnitude < step * step)
        {
            transform.position = targetPosition;
        }
        else
        {
            transform.position += distance.normalized * step;
            flyingTime += Time.deltaTime;
        }

        if (flyingTime > 1)
        {
            int fuel = Mathf.FloorToInt(flyingTime);
            flyingTime -= fuel;
            resourceManager.UpdateStockpile(ResourceType.Fuel, -fuel);
        }
    }

    public void OnClick(Vector2 position)
    {
        var target = Camera.main.ScreenToWorldPoint(position);
        targetPosition = new Vector3(target.x, target.y, transform.position.z);
    }

    // TODO: Make damage taken more visible.
    public bool UpdateHealth(int amount) => resourceManager.UpdateStockpile(ResourceType.Health, amount);
}
