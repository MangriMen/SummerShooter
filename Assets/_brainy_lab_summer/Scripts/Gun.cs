using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField]
    private GameObject bullet;

    [SerializeField]
    private Transform shotPoint;

    [SerializeField]
    private float shotDelay;

    private float shotTimer;

    void Start()
    {
        bullet.GetComponent<SpriteRenderer>().color = GetComponent<SpriteRenderer>().color;
    }

    void Update()
    {
        if (shotTimer > 0)
        {
            shotTimer -= Time.deltaTime;
        }
    }

    public void Shoot()
    {
        if (shotTimer <= 0)
        {
            Instantiate(bullet, shotPoint.position, transform.rotation);
            shotTimer = shotDelay;
        }
    }
}
