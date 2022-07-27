using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class CharacterController : MonoBehaviour
{
    [SerializeField]
    protected float _speed = 2;

    [SerializeField]
    protected float _rotationSpeed = 90;

    [SerializeField]
    protected string _name;

    [SerializeField]
    protected Color _color;

    protected Rigidbody2D _rb;
    protected Gun _gun;
    protected Vector2 _velocity;

    public string Name { get => _name; protected set => _name = value; }
    public Color Color { get => _color; protected set => _color = value; }
    public bool IsAlive { get; protected set; } = true;

    protected void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _gun = GetComponentInChildren<Gun>();

        _gun.SetColor(_color);
    }

    protected void FixedUpdate()
    {
        _rb.MovePosition(_rb.position + _velocity * Time.fixedDeltaTime);
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
