using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectResources : MonoBehaviour
{
    private Base playerBase;
    private ResourceManager resourceManager;

    public void Start()
    {
        playerBase = FindObjectOfType<Base>();
        resourceManager = FindObjectOfType<ResourceManager>();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        var resource = other.GetComponent<Resource>();
        if (resource != null) FoundResource(resource);
    }

    private void FoundResource(Resource resource)
    {
        if (!resourceManager.Add(resource.Type)) { return; }
        resource.Collect(playerBase);
    }
}
