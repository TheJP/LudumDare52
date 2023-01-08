using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SmallTurret : Turret
{
    private const string EnemyTag = "Enemy";
    private const string ProjectileParentTag = "ProjectileParent";

    [SerializeField]
    private SmallTurretProjectile projectilePrefab;

    [SerializeField]
    private float shootingCooldown = 1f;

    [SerializeField]
    private float turretRange = 5f;

    private Transform projectileParent;
    private float lastShot = 0f;

    public void Start()
    {
        projectileParent = GameObject.FindWithTag(ProjectileParentTag).transform;
    }

    public void Update()
    {
        if (Time.time - lastShot < shootingCooldown) { return; }
        lastShot = Time.time;

        var results = Physics2D.OverlapCircleAll(transform.position, turretRange);
        Transform closest = null;
        float closestDistance = float.PositiveInfinity;
        foreach (var result in results.Where(r => r.CompareTag(EnemyTag)))
        {
            var distance = (result.transform.position - transform.position).sqrMagnitude;
            if (distance < closestDistance)
            {
                closest = result.transform;
                closestDistance = distance;
            }
        }

        if (closest == null) { return; }

        var projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity, projectileParent);
        projectile.Target = closest;
    }
}
