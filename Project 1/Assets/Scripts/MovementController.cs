using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    Vector3 objectPosition = Vector3.zero;

    [SerializeField]
    float speed = 1.0f;

    Vector3 direction = Vector3.zero;

    Vector3 velocity = Vector3.zero;

    public float Speed { get { return speed; } set { speed = value; } }

    // Start is called before the first frame update
    void Start()
    {
        objectPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        velocity = direction * speed * Time.deltaTime;

        objectPosition += velocity;

        // Checks if player moves off screen
        // - Right
        if (objectPosition.x >= 16)
        {
            objectPosition = new Vector3(-15.9f, objectPosition.y, objectPosition.z);
        }

        // - Left
        if (objectPosition.x <= -16)
        {
            objectPosition = new Vector3(16f, objectPosition.y, objectPosition.z);
        }

        transform.position = objectPosition;
    }

    public void SetDirection(Vector3 newDirection)
    {
        direction = newDirection.normalized;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + direction*2);
    }
}
