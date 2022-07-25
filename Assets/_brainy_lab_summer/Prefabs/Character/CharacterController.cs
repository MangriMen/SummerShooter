using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterController : MonoBehaviour
{
    [SerializeField]
    protected float speed = 2;

    [SerializeField]
    protected float rotationAngle = 90;

    protected Rigidbody2D rb;
    protected Gun gun;

    protected Vector2 velocity;

    public bool IsAlive { get; protected set; } = true;

    public string nickname;
    public Color color;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gun = GetComponentInChildren<Gun>();
        gun.SendMessage("SetColor", color);
    }

    protected void FixedUpdate()
    {
        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
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
