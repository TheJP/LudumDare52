using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[Serializable]
public class ResourceManagerEntry
{
    [field: SerializeField]
    public ResourceType Type { get; set; }

    [field: SerializeField]
    public int Value { get; set; }
}

[ExecuteInEditMode]
public class ResourceManager : MonoBehaviour
{
    [SerializeField]
    private UIDocument resourceDisplay;

    [SerializeField]
    private ResourceManagerEntry[] resources;

    [SerializeField]
    private ResourceManagerEntry[] resourceMaximum;

    private readonly Dictionary<ResourceType, Label> labels = new();
    private readonly Dictionary<ResourceType, ProgressBar> bars = new();

    private static readonly IReadOnlyCollection<(ResourceType type, string name)> names = Array.AsReadOnly(new[]
    {
        (ResourceType.BuildingMaterial, "building-material"),
        (ResourceType.Fuel, "fuel"),
        (ResourceType.Health, "health"),
        (ResourceType.Research, "research"),
    });

    public void Start()
    {
        foreach (var resource in names)
        {
            var label = resourceDisplay.rootVisualElement.Q<Label>($"{resource.name}-label");
            var bar = resourceDisplay.rootVisualElement.Q<ProgressBar>($"{resource.name}-bar");
            if (label != null) { labels.Add(resource.type, label); }
            if (bar != null) { bars.Add(resource.type, bar); }
        }
    }

    public void Update()
    {
        foreach (var max in resourceMaximum)
        {
            if (bars.ContainsKey(max.Type))
            {
                bars[max.Type].highValue = max.Value;
            }
        }

        foreach (var resource in resources)
        {
            if (labels.ContainsKey(resource.Type))
            {
                labels[resource.Type].text = $"{resource.Value}";
            }
            if (bars.ContainsKey(resource.Type))
            {
                bars[resource.Type].value = resource.Value;
            }
        }
    }
}
