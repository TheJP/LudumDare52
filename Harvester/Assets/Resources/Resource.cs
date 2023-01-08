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
[RequireComponent(typeof(Rigidbody2D))]
public class Resource : MonoBehaviour
{
    private Rigidbody2D body;

    [field: SerializeField]
    public ResourceType Type { get; private set; }

    [SerializeField]
    private ResourceSpritePair[] sprites;

    [SerializeField]
    private float collectSpeed = 10f;

    private Base targetBase = null;
    public bool Collected => targetBase != null;
    public event Action OnCollected;

    private ResourceType currentType = ResourceType.None;

    public void Start() => body = GetComponent<Rigidbody2D>();

    public void Update()
    {
        Debug.Assert(Type != ResourceType.None, "Invalid resource blob: resource type is 'none'");
        if (currentType != Type && Type != ResourceType.None) { UpdateResourceType(Type); }
    }

    public void FixedUpdate()
    {
        if (targetBase == null) { return; }

        var distance = targetBase.transform.position - transform.position;
        var stepDistance = collectSpeed * Time.fixedDeltaTime;
        if (distance.sqrMagnitude >= stepDistance * stepDistance)
        {
            body.velocity = distance.normalized * collectSpeed;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateResourceType(ResourceType type)
    {
        Debug.Assert(!Collected, "Resource type was changed after it has been collected");
        Type = type;
        currentType = type;
        foreach (var sprite in sprites) { sprite.Sprite.gameObject.SetActive(false); }
        sprites.First(pair => pair.Type == currentType).Sprite.gameObject.SetActive(true);
    }

    public bool Collect(Base targetBase)
    {
        if (Collected) { return false; }
        this.targetBase = targetBase;
        OnCollected?.Invoke();
        return true;
    }
}
