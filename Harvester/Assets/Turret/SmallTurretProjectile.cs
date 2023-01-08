using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SmallTurretProjectile : MonoBehaviour
{
    private Rigidbody2D body;

    [SerializeField]
    private float speed = 8f;

    public Transform Target { get; private set; }

    public float Damage { get; private set; }

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
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan2(direction.y, direction.x));
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (Target == null) { return; }
        if (Target == other.transform)
        {
            Target.GetComponent<EnemyHealth>().UpdateHealth(-Damage);
            Damage = 0; // Prevent applying damage multiple times.
            Destroy(gameObject);
        }
    }

    public void SetTarget(Transform target, float damage)
    {
        Damage = damage;
        Target = target;
    }
}
