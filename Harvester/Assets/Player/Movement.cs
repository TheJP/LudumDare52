using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
    private Rigidbody2D body;

    [SerializeField]
    private float speed = 1f;

    [SerializeField]
    private float turnSpeed = 1f;

    [SerializeField]
    private Transform turnObject;

    private Vector2 move = Vector2.zero;

    // Rotation animation
    private float rotation = 0f;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        rotation = turnObject.eulerAngles.z;
    }

    public void FixedUpdate()
    {
        body.velocity = move * speed;
        if (move.sqrMagnitude > 0.1) { Rotate(); }
    }

    private void Rotate()
    {
        float targetRotation = Mathf.Rad2Deg * Mathf.Atan2(move.y, move.x);
        float rotationDistance = Mathf.DeltaAngle(rotation, targetRotation);
        if (Mathf.Abs(rotationDistance) < turnSpeed * Time.deltaTime)
        {
            rotation = targetRotation;
        }
        else
        {
            var rotateDirection = Mathf.Sign(Mathf.DeltaAngle(rotation, targetRotation));
            rotation += rotateDirection * turnSpeed * Time.deltaTime;
        }
        turnObject.rotation = Quaternion.Euler(0, 0, rotation);
    }

    public void OnMove(InputValue value)
    {
        move = value.Get<Vector2>();
        body.velocity = move * speed;
    }
}
