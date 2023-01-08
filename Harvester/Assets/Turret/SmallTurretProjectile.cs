using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SmallTurretProjectile : Projectile
{
    protected override void HitEnemy() => Target.GetComponent<EnemyHealth>().UpdateHealth(-Damage);
}
