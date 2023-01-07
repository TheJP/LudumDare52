using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectResources : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D other)
    {
        var resource = other.GetComponent<Resource>();
        if (resource != null) FoundResource(resource);
    }

    private void FoundResource(Resource resource)
    {
        resource.transform.position = new Vector3(10, 10, 0);
    }
}
