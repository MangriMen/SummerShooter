using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float _speed;

    private Rigidbody2D _rb;
    private GameObject _owner;
    private Vector2 _velocity;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();

        _velocity = transform.up * _speed;
    }

    void FixedUpdate()
    {
        _rb.MovePosition(_rb.position + _velocity * Time.fixedDeltaTime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == _owner)
        {
            return;
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.collider.GetComponent<Enemy>().GetShot();
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            collision.collider.GetComponent<Player>().GetShot();
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            Vector2 wallNormal = collision.contacts[0].normal;
            Vector2 newDirection = Vector2.Reflect(_velocity, wallNormal).normalized;
            _velocity = newDirection * _speed;
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
        _owner = gameObject;
    }
}
