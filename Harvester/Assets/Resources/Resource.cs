using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
    [field: SerializeField]
    public ResourceType Type { get; private set; }

    [SerializeField]
    private ResourceSpritePair[] sprites;

    private ResourceType currentType = ResourceType.None;

    //private Dictionary<ResourceType, SpriteRenderer> spriteMap;
    //public void Awake() => spriteMap = sprites.ToDictionary(pair => pair.Type, pair => pair.Sprite);

    public void Update()
    {
        Debug.Assert(Type != ResourceType.None, "Invalid resource blob: resource type is 'none'");
        if (currentType != Type && Type != ResourceType.None) { UpdateResourceType(Type); }
    }

    public void UpdateResourceType(ResourceType type)
    {
        Type = type;
        currentType = type;
        foreach (var sprite in sprites) { sprite.Sprite.gameObject.SetActive(false); }
        sprites.First(pair => pair.Type == currentType).Sprite.gameObject.SetActive(true);
    }
}
