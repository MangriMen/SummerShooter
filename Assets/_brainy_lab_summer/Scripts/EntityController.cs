using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityController : MonoBehaviour
{
    [SerializeField]
    protected float speed = 2;

    [SerializeField]
    protected float rotationAngle = 90;

    protected Rigidbody2D rb;
    protected Gun gun;

    protected Vector2 moveInput;

    protected Vector2 velocity;
    protected float rotation;

    public bool IsAlive { get; protected set; } = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gun = GetComponentInChildren<Gun>();
    }

    protected void FixedUpdate()
    {
        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        rb.MoveRotation(rb.rotation + rotation * Time.fixedDeltaTime);
    }

    protected Vector2 GetVector2FromAngle(float angle)
    {
        float radAngle = angle * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(radAngle), Mathf.Sin(radAngle));
    }

    protected void Kill()
    {
        IsAlive = false;
    }

    public void Revive()
    {
        IsAlive = true;
    }
}
