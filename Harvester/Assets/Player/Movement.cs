using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
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
    private Base playerBase;

    // Rotation animation
    private float rotation = 0f;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        rotation = turnObject.eulerAngles.z;
        playerBase = FindObjectOfType<Base>();
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
        if (Mathf.Abs(rotationDistance) < turnSpeed * Time.fixedDeltaTime)
        {
            rotation = targetRotation;
        }
        else
        {
            var rotateDirection = Mathf.Sign(Mathf.DeltaAngle(rotation, targetRotation));
            rotation += rotateDirection * turnSpeed * Time.fixedDeltaTime;
        }
        turnObject.rotation = Quaternion.Euler(0, 0, rotation);
    }

    public void OnMove(InputValue value)
    {
        move = value.Get<Vector2>();
        body.velocity = move * speed;
    }


    private Vector2 pointer = Vector2.zero;
    public void OnPoint(InputValue value) => pointer = value.Get<Vector2>();
    // We have to defer call to IsPointerOverGameObject, because it should not be called during event handling.
    public void OnClick() => StartCoroutine(DelayedClick());
    private IEnumerator DelayedClick()
    {
        yield return null;
        if (EventSystem.current.IsPointerOverGameObject()) { yield break; }
        playerBase.OnClick(pointer);
    }
}
