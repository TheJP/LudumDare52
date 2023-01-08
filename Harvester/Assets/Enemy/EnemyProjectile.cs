using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : Projectile
{
    protected override void HitEnemy(int damage)
    {
        Target.GetComponent<Base>().UpdateHealth(-damage);
        Destroy(gameObject, 0.1f);
    }
}
