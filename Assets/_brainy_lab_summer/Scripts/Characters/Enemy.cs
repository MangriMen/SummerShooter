using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class Enemy : CharacterController
{
    public float distance;

    private bool needShoot;

    [SerializeField]
    private LevelController levelController;

    private void Update()
    {
        velocity = new Vector2(-1, 0) * speed;
        RotateToPlayer();

        if (CheckPlayer())
        {
            TryToShoot();
        }
    }

    new private void FixedUpdate()
    {
        base.FixedUpdate();
        rb.MoveRotation(
            Quaternion.RotateTowards(
                Quaternion.Euler(0, 0, rb.rotation),
                Quaternion.Euler(0, 0, TransformUtils.Angle(levelController.player.transform, transform)),
                rotationAngle * Time.deltaTime
                ));
    }

    public void RotateToPlayer()
    {

        // transform.right = levelController.player.transform.position - transform.position;
    }

    private bool CheckPlayer()
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
                return true;
            }
        }

        return false;
    }

    private void TryToShoot()
    {
        if (Random.Range(1, 100) < 30)
        {
            gun.Shoot(2, 0.1f);
        }
        else
        {
            gun.Shoot();
        }
    }

    public void TakeShot()
    {
        Kill();
    }
}
