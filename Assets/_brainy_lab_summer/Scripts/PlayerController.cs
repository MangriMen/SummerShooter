using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : EntityController
{
    void Update()
    {
        moveInput = GetVector2FromAngle(transform.rotation.eulerAngles.z) * Input.GetAxisRaw("Vertical");

        velocity = moveInput * speed;
        rotation = -Input.GetAxisRaw("Horizontal") * rotationAngle;
    }
}
