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
    private Transform projectileSpawnPosition;

    [SerializeField]
    private float shootingCooldown = 1f;

    [SerializeField]
    private float turretRange = 5f;

    [SerializeField]
    private float turnSpeed = 10f;

    [SerializeField]
    private float aquireTargetCooldown = 1f;

    private Transform projectileParent;
    private float lastShot = 0f;
    private Transform currentTarget = null;
    private float currentAngle;
    private float aquireTargetTime = 0f;

    public void Start()
    {
        projectileParent = GameObject.FindWithTag(ProjectileParentTag).transform;
        currentAngle = transform.eulerAngles.z;
    }

    public void Update()
    {
        if (currentTarget == null || Vector2.Distance(currentTarget.position, transform.position) > turretRange)
        {
            // Cooldown is added because AquireTarget is an expensive operation.
            if (Time.time - aquireTargetTime < aquireTargetCooldown) { return; }
            currentTarget = AquireTarget();
            aquireTargetTime = Time.time; // Go on cooldown no matter if target was found or not.
        }
        if (currentTarget == null) { return; }

        // Turn turret to face target
        var distance = currentTarget.position - transform.position;
        var targetAngle = Mathf.Rad2Deg * Mathf.Atan2(distance.y, distance.x);
        var deltaAngle = Mathf.DeltaAngle(currentAngle, targetAngle);
        if (Mathf.Abs(deltaAngle) > turnSpeed * Time.deltaTime)
        {
            currentAngle += deltaAngle * turnSpeed * Time.deltaTime;
        }
        else
        {
            currentAngle = targetAngle;

            // Fire turret after reaching target angle
            if (Time.time - lastShot < shootingCooldown) { return; }
            lastShot = Time.time;
            var spawnPosition = (projectileSpawnPosition != null ? projectileSpawnPosition : transform).position;
            var projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.Euler(0, 0, currentAngle), projectileParent);
            projectile.Target = currentTarget;
        }

        transform.rotation = Quaternion.Euler(0, 0, currentAngle);
    }

    private Transform AquireTarget()
    {
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

        return closest;
    }
}
