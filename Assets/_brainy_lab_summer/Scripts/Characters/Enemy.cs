using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : EntityController
{
    public float distance;

    private void Update()
    {
        var hitInfo = Physics2D.Raycast(
            gun.shotPoint.transform.position,
            gun.shotPoint.transform.up,
            distance
        );

        if (hitInfo.collider != null)
        {
            if (hitInfo.collider.CompareTag("Player"))
            {
                gun.Shoot();
            }
        }
    }

    public void TakeShot()
    {
        Kill();
    }
}
