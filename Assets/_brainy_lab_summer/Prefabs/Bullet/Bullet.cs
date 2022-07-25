using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float speed;

    [SerializeField]
    private float distance;

    protected Rigidbody2D rb;

    protected Vector2 velocity;

    protected GameObject owner;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        velocity = transform.up * speed;
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == owner)
        {
            return;
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.collider.GetComponent<Enemy>().TakeShot();
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            collision.collider.GetComponent<Player>().TakeShot();
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            Vector2 wallNormal = collision.contacts[0].normal;

            Vector2 newDirection = Vector2.Reflect(velocity, wallNormal).normalized;

            velocity = newDirection * speed;
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    public void SetColor(Color color)
    {
        GetComponent<SpriteRenderer>().color = color;
    }

    public void SetOwner(GameObject gameObject)
    {
        owner = gameObject;
    }
}
