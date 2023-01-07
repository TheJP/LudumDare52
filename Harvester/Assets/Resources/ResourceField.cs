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

    private ResourceType currentType = ResourceType.None;

    public event Action OnCollected;

    public void Start()
    {
        foreach(var resource in GetComponentsInChildren<Resource>())
        {
            resource.OnCollected += CheckEmpty;
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

    private void CheckEmpty()
    {
        if (GetComponentsInChildren<Resource>().All(r => r.Collected))
        {
            OnCollected?.Invoke();
        }
    }
}
