using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResourceFieldManager : MonoBehaviour
{
    [SerializeField]
    private ResourceField[] fieldPrefabs;

    [SerializeField]
    private float spawnRange = 100f;

    private List<ResourceField> fields = new();

    public void Update()
    {
        foreach (var type in ResourceTypeExtensions.Types)
        {
            if (fields.Count(f => f.Type == type) == 0)
            {
                SpawnField(type);
            }
        }
    }

    private void SpawnField(ResourceType type)
    {
        var prefab = fieldPrefabs[Random.Range(0, fieldPrefabs.Length)];
        Debug.Assert(prefab != null);

        var newField = Instantiate(prefab, Random.insideUnitCircle * spawnRange, Quaternion.identity, transform);
        newField.UpdateResourceType(type);
        fields.Add(newField);
    }
}
