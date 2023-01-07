using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum ResourceType
{
    None,
    BuildingMaterial,
    Fuel,
    Health,
    Research,
}

[Serializable]
public class ResourceSpritePair
{
    [field: SerializeField]
    public ResourceType Type { get; set; }

    [field: SerializeField]
    public SpriteRenderer Sprite { get; set; }
}

[ExecuteInEditMode]
public class Resource : MonoBehaviour
{
    [SerializeField]
    private ResourceType type;

    [SerializeField]
    private ResourceSpritePair[] sprites;

    private ResourceType currentType = ResourceType.None;

    //private Dictionary<ResourceType, SpriteRenderer> spriteMap;
    //public void Awake() => spriteMap = sprites.ToDictionary(pair => pair.Type, pair => pair.Sprite);

    public void Update()
    {
        Debug.Assert(type != ResourceType.None, "Invalid resource blob: resource type is 'none'");

        if (currentType == type) { return; }
        currentType = type;
        foreach (var sprite in sprites) { sprite.Sprite.gameObject.SetActive(false); }
        sprites.First(pair => pair.Type == type).Sprite.gameObject.SetActive(true);
    }
}
