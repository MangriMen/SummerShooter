using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField]
    private GameObject bullet;

    public Transform shotPoint;

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

    public async void Shoot(uint count = 1, float delay = 0)
    {
        if (shotTimer <= 0)
        {
            for (uint i = 0; i < count; i++)
            {
                CreateBullet();
                await Task.Delay((int)(delay * 1000));
            }

            shotTimer = shotDelay;
        }
    }

    private GameObject CreateBullet()
    {
        GameObject go = Instantiate(bullet, shotPoint.position, transform.rotation);
        go.SendMessage("SetColor", Color);
        go.SendMessage("SetOwner", transform.parent.gameObject);
        return go;
    }

    public void SetColor(Color color)
    {
        GetComponent<SpriteRenderer>().color = color;
    }
}
