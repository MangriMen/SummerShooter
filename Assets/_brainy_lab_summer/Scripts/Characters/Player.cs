using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : CharacterController
{
    private float rotation;
    void Update()
    {
        velocity = transform.right * Input.GetAxisRaw("Vertical") * speed;
        rotation = -Input.GetAxisRaw("Horizontal") * rotationAngle;

        if (Input.GetKey(KeyCode.Space))
        {
            gun.Shoot();
        }
    }

    new void FixedUpdate()
    {
        base.FixedUpdate();
        rb.MoveRotation(rb.rotation + rotation * Time.deltaTime);
    }

    public void TakeShot()
    {
        Kill();
    }
}
