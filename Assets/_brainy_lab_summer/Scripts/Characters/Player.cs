using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class Player : CharacterController
{
    protected Vector2 moveInput;

    void Update()
    {
        moveInput =
            Vector2Utils.FromAngle(transform.rotation.eulerAngles.z) * Input.GetAxisRaw("Vertical");

        velocity = moveInput * speed;
        rotation = -Input.GetAxisRaw("Horizontal") * rotationAngle;

        if (Input.GetKey(KeyCode.Space))
        {
            gun.Shoot();
        }
    }

    public void TakeShot()
    {
        Kill();
    }
}
