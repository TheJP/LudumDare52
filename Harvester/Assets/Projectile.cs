using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class Projectile : MonoBehaviour
{
    protected abstract void HitEnemy(int damage);


    private Rigidbody2D body;

    [SerializeField]
    private float speed = 8f;

    [SerializeField]
    private float spinSpeed = 0f;

    public Transform Target { get; private set; }

    public int Damage { get; private set; }

    public void Start() => body = GetComponent<Rigidbody2D>();

    public void FixedUpdate()
    {
        if (Target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector2 direction = Target.position - transform.position;
        body.velocity = direction.normalized * speed;
        if (spinSpeed < 0.001f)
        {
            transform.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan2(direction.y, direction.x));
        }
        else
        {
            transform.Rotate(0, 0, spinSpeed * Time.fixedDeltaTime);
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (Target == null) { return; }
        if (other.transform.IsChildOf(Target)) // true if either ancestor or equal.
        {
            if (Damage != 0) { HitEnemy(Damage); }
            Damage = 0; // Prevent applying damage multiple times.
        }
    }

    public void SetTarget(Transform target, int damage)
    {
        Damage = damage;
        Target = target;
    }
}
