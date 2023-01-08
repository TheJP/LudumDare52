using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MeleeEnemy : MonoBehaviour
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

    private Base playerBase;

    private bool attacking = false;

    public void Start()
    {
        body = GetComponent<Rigidbody2D>();
        playerBase = FindObjectOfType<Base>();
    }

    public void FixedUpdate()
    {
        transform.Rotate(0, 0, (attacking ? turnSpeedAttacking : turnSpeed) * Time.fixedDeltaTime);

        var distance = playerBase.transform.position - transform.position;
        if (distance.sqrMagnitude > range * range)
        {
            body.velocity = Vector2.zero;
            return;
        }

        body.velocity = distance.normalized * speed;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponentInParent<Base>() != null) { attacking = true; }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponentInParent<Base>() != null) { attacking = false; }
    }
}
