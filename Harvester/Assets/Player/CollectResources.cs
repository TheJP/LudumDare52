using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectResources : MonoBehaviour
{
    private Base playerBase;

    public void Start()
    {
        playerBase = FindObjectOfType<Base>();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        var resource = other.GetComponent<Resource>();
        if (resource != null) FoundResource(resource);
    }

    private void FoundResource(Resource resource)
    {
        resource.Collect(playerBase);
        // TODO: Add resource in manager
    }
}
