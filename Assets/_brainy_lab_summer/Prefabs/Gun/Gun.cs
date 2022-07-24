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

    private SpriteRenderer sr;

    public Color Color
    {
        get => sr.color;
    }

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
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
            GameObject go = Instantiate(bullet, shotPoint.position, transform.rotation);
            go.SendMessage("SetColor", Color);
            go.SendMessage("SetOwner", transform.parent.gameObject);

            shotTimer = shotDelay;
        }
    }

    public void SetColor(Color color)
    {
        GetComponent<SpriteRenderer>().color = color;
    }
}
