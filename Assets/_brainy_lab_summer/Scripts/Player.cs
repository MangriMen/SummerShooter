using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : EntityController
{
    void Update()
    {
        moveInput =
            GetVector2FromAngle(transform.rotation.eulerAngles.z) * Input.GetAxisRaw("Vertical");

        velocity = moveInput * speed;
        rotation = -Input.GetAxisRaw("Horizontal") * rotationAngle;

        if (Input.GetKey(KeyCode.Space))
        {
            gun.Shoot();
        }
    }

    public void TakeDamage()
    {
        Kill();
    }
}
