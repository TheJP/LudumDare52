using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [field: SerializeField]
    public float Health { get; private set; } = 100f;

    [field: SerializeField]
    public float MaxHealth { get; private set; } = 100f;

    public void UpdateHealth(float amount)
    {
        Health = Mathf.Clamp(Health + amount, 0, MaxHealth);
        if (Health < 0.001) { Destroy(gameObject); }
    }
}
