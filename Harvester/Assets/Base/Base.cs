using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField]
    private float speed = 1f;

    private Vector3 targetPosition;

    public void Start() => targetPosition = transform.position;

    public void Update()
    {
        var click = Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.touchCount > 0;
        if (click)
        {
            var position = Input.touchCount > 0 ? Input.touches[0].position : (Vector2)Input.mousePosition;
            var target = Camera.main.ScreenToWorldPoint(position);
            targetPosition = new Vector3(target.x, target.y, transform.position.z);
        }

        var distance = targetPosition - transform.position;
        var step = speed * Time.deltaTime;
        if (distance.sqrMagnitude < step * step)
        {
            transform.position = targetPosition;
        }
        else
        {
            transform.position += distance.normalized * step;
        }
    }
}
