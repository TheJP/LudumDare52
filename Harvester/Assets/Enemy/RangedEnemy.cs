using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class RangedEnemy : MonoBehaviour
{
    private Rigidbody2D body;

    [SerializeField]
    private float speed = 4f;

    [SerializeField]
    private float turnSpeed = 45f;

    [SerializeField]
    private float targetRange = 15f;

    [SerializeField]
    private float attackRange = 5f;

    [SerializeField]
    private float flyingRange = 3f;

    [SerializeField]
    private float rangeBand = 2f;

    [SerializeField]
    private float attackCooldown = 2f;

    //[SerializeField]
    //private 

    private Base playerBase;
    private float lastAttackTime;
    private bool flyAway = false;
    private bool flyCloser = false;

    private float currentAngle;

    public void Start()
    {
        body = GetComponent<Rigidbody2D>();
        playerBase = FindObjectOfType<Base>();
        currentAngle = transform.eulerAngles.z;
    }

    public void Update()
    {

    }

    private void FixedUpdate()
    {
        Vector2 distance = playerBase.transform.position - transform.position;
        var squaredDistance = distance.sqrMagnitude;

        if (squaredDistance <= targetRange * targetRange)
        {
            var targetAngle = Mathf.Rad2Deg * Mathf.Atan2(distance.y, distance.x);
            var deltaAngle = Mathf.DeltaAngle(currentAngle, targetAngle);
            if (Mathf.Abs(deltaAngle) < turnSpeed * Time.fixedDeltaTime) { currentAngle = targetAngle; }
            else { currentAngle += Mathf.Sign(deltaAngle) * turnSpeed * Time.fixedDeltaTime; }
            transform.rotation = Quaternion.Euler(0, 0, currentAngle);
        }

        if (squaredDistance > targetRange * targetRange)
        {
            body.velocity = Vector2.zero;
        }
        else if (squaredDistance > attackRange * attackRange)
        {
            body.velocity = distance.normalized * speed;
            lastAttackTime = Time.time;
        }
        else
        {
            // Oscillate between attackRange and flyingRange.
            float middleRange = (attackRange + flyingRange) * 0.5f;
            float attackBandRange = middleRange + rangeBand * 0.5f;
            float flyingBandRange = middleRange - rangeBand * 0.5f;
            if (squaredDistance > attackBandRange * attackBandRange) { flyCloser = true; }
            if (squaredDistance < flyingBandRange * flyingBandRange) { flyAway = true; }

            if (flyAway)
            {
                if (squaredDistance < middleRange * middleRange)
                {
                    body.velocity = distance.normalized * -speed;
                }
                else
                {
                    flyAway = false;
                }
            }
            else if (flyCloser)
            {
                if (squaredDistance > middleRange * middleRange)
                {
                    body.velocity = distance.normalized * speed;
                }
                else
                {
                    flyCloser = false;
                }
            }
            else
            {
                body.velocity = Vector2.zero;
            }

            if (Time.time - lastAttackTime < attackCooldown) { return; }
            lastAttackTime += attackCooldown;
            // TODO: Attack
            Debug.Log("attack");
        }
    }
}
