using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MeleeEnemy : Enemy
{
    private Rigidbody2D body;

    [SerializeField]
    private float range = 15f;

    [SerializeField]
    private float speed = 4f;

    [SerializeField]
    private float turnSpeed = 20f;

    [SerializeField]
    private float turnSpeedAttacking = 180f;

    [SerializeField]
    private int attackDamage = 1;

    [SerializeField]
    private float attackCooldown = 0.5f;

    private Base playerBase;

    private bool attacking = false;
    private float lastAttackTime = 0;

    public void Start()
    {
        body = GetComponent<Rigidbody2D>();
        playerBase = FindObjectOfType<Base>();
    }

    public void Update()
    {
        if (!attacking) { return; }
        if (Time.time - lastAttackTime < attackCooldown) { return; }

        lastAttackTime += attackCooldown;
        playerBase.UpdateHealth(-attackDamage);
    }

    public void FixedUpdate()
    {
        transform.Rotate(0, 0, (attacking ? turnSpeedAttacking : turnSpeed) * Time.fixedDeltaTime);
        Vector2 distance = playerBase.transform.position - transform.position;

        if (Protectee == null)
        {
            if (distance.sqrMagnitude > range * range)
            {
                body.velocity = Vector2.zero;
                return;
            }
        }
        else
        {
            if (!AttackedByPlayer)
            {
                body.velocity = Vector2.zero;
                return;
            }
        }

        body.velocity = distance.normalized * speed;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponentInParent<Base>() != null)
        {
            attacking = true;
            lastAttackTime = Time.time;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponentInParent<Base>() != null) { attacking = false; }
    }
}
