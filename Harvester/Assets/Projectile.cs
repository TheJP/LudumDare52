using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class Projectile : MonoBehaviour
{
    protected abstract void HitEnemy();


    private Rigidbody2D body;

    [SerializeField]
    private float speed = 8f;

    [SerializeField]
    private bool spinning = false;

    public Transform Target { get; private set; }

    public int Damage { get; private set; }

    public void Start() => body = GetComponent<Rigidbody2D>();

    public void FixedUpdate()
    {
        if (Target == null)
        {
            body.velocity = Vector3.zero;
            return;
        }

        Vector2 direction = Target.position - transform.position;
        body.velocity = direction.normalized * speed;
        if (!spinning)
        {
            transform.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan2(direction.y, direction.x));
        }
        else
        {
            //TODO:
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (Target == null) { return; }
        if (Target == other.transform)
        {
            if (Damage != 0) { HitEnemy(); }
            Damage = 0; // Prevent applying damage multiple times.
            Destroy(gameObject);
        }
    }

    public void SetTarget(Transform target, int damage)
    {
        Damage = damage;
        Target = target;
    }
}
