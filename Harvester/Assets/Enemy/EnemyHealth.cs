using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyHealth : MonoBehaviour
{
    [field: SerializeField]
    public int Health { get; private set; } = 100;

    [field: SerializeField]
    public int MaxHealth { get; private set; } = 100;

    private Enemy enemy;

    public void Start()
    {
        enemy = GetComponent<Enemy>();
    }

    public void UpdateHealth(int amount)
    {
        // Got attacked: Acticate field defenses if this is a protector.
        if (amount < 0 && enemy != null && enemy.Protectee != null)
        {
            enemy.Protectee.GotAttacked();
        }

        Health = Mathf.Clamp(Health + amount, 0, MaxHealth);
        if (Health == 0) { Destroy(gameObject); }
    }
}
