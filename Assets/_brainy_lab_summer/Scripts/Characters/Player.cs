using UnityEngine;

public class Player : CharacterController
{
    private float _rotation;

    void Update()
    {
        _velocity = transform.right * Input.GetAxisRaw("Vertical") * _speed;
        _rotation = -Input.GetAxisRaw("Horizontal") * _rotationSpeed;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _gun.Shoot();
        }
    }

    new void FixedUpdate()
    {
        base.FixedUpdate();
        _rb.MoveRotation(_rb.rotation + _rotation * Time.deltaTime);
    }

    public void GetShot()
    {
        Kill();
    }
}
