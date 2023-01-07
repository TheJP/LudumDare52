using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    private readonly HashSet<ResourceType> maxedOut = new();
    private readonly Dictionary<ResourceType, Color> defaultColours = new();

    private static readonly IReadOnlyCollection<(ResourceType type, string name)> names = Array.AsReadOnly(new[]
    {
        (ResourceType.BuildingMaterial, "building-material"),
        (ResourceType.Fuel, "fuel"),
        (ResourceType.Health, "health"),
        (ResourceType.Research, "research"),
    });

    private int MaxValue(ResourceType type) => resourceMaximum.FirstOrDefault(r => r.Type == type)?.Value ?? 0;

    public void Start()
    {
        foreach (var resource in names)
        {
            var label = resourceDisplay.rootVisualElement.Q<Label>($"{resource.name}-label");
            var bar = resourceDisplay.rootVisualElement.Q<ProgressBar>($"{resource.name}-bar");
            if (bar != null) { bars.Add(resource.type, bar); }
            if (label != null)
            {
                labels.Add(resource.type, label);
                defaultColours.Add(resource.type, label.resolvedStyle.color);
            }
        }

        foreach (var resource in resources)
        {
            var max = MaxValue(resource.Type);
            if (resource.Value >= max)
            {
                resource.Value = max;
                maxedOut.Add(resource.Type);
            }
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

        if (!Application.isPlaying) { return; }
        Debug.Log("playing");

        foreach (var resource in resources)
        {
            if (!labels.ContainsKey(resource.Type)) { continue; }
            var label = labels[resource.Type];
            var isRed = Mathf.RoundToInt(Time.time) % 2 == 0 && maxedOut.Contains(resource.Type);
            label.style.color = new StyleColor(isRed ? Color.red : defaultColours[resource.Type]);
        }
    }

    public bool UpdateStockpile(ResourceType type, int amount = 1)
    {
        foreach (var resource in resources)
        {
            if (resource.Type != type) { continue; }
            var max = MaxValue(type);
            var newValue = resource.Value + amount;
            if (0 <= newValue && newValue <= max)
            {
                resource.Value = newValue;
                if (resource.Value == max) { maxedOut.Add(resource.Type); }
                else { maxedOut.Remove(resource.Type); }
                return true;
            }
        }

        return false;
    }
}
