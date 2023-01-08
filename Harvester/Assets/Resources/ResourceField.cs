using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class ResourceField : MonoBehaviour
{
    [field: SerializeField]
    public ResourceType Type { get; private set; }

    [field: SerializeField]
    public Transform EnemySpawnParent { get; private set; }

    private ResourceType currentType = ResourceType.None;

    public event Action OnCollected;

    private readonly List<Enemy> protectors = new();
    private bool gotAttackedBefore = false;

    public void Start()
    {
        foreach (var resource in GetComponentsInChildren<Resource>())
        {
            resource.OnCollected += OnResourceBlobCollected;
        }
    }

    public void Update()
    {
        Debug.Assert(Type != ResourceType.None, "Resource field has invalid resource type 'none'");
        if (currentType != Type && Type != ResourceType.None) { UpdateResourceType(Type); }
    }

    public void UpdateResourceType(ResourceType type)
    {
        Type = type;
        currentType = type;
        foreach (var resource in GetComponentsInChildren<Resource>())
        {
            resource.UpdateResourceType(currentType);
        }
    }

    private void OnResourceBlobCollected()
    {
        GotAttacked(); // Resource "stealing" is treated as attack.
        if (GetComponentsInChildren<Resource>().All(r => r.Collected))
        {
            OnCollected?.Invoke();
        }
    }

    public void GotAttacked()
    {
        if (gotAttackedBefore) { return; }
        gotAttackedBefore = true;

        foreach (var protector in protectors.Where(p => p != null))
        {
            protector.GotAttacked();
        }
    }

    public void AddProtector(Enemy enemy) => protectors.Add(enemy);
}
