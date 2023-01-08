using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [field: SerializeField]
    public int Health { get; private set; } = 100;

    [field: SerializeField]
    public int MaxHealth { get; private set; } = 100;

    public void UpdateHealth(int amount)
    {
        Health = Mathf.Clamp(Health + amount, 0, MaxHealth);
        if (Health == 0) { Destroy(gameObject); }
    }
}
