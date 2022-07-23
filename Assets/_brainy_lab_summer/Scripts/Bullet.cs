using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float speed;

    [SerializeField]
    private float lifetime;

    [SerializeField]
    private float distance;

    public int damage;

    void Start() { }

    void Update()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.up, distance);

        if (hitInfo.collider != null)
        {
            if (hitInfo.collider.CompareTag("Enemy"))
            {
                hitInfo.collider.GetComponent<Enemy>().TakeDamage();
                Destroy(gameObject);
            }
            else if (hitInfo.collider.CompareTag("Player"))
            {
                hitInfo.collider.GetComponent<Player>().TakeDamage();
            }
            else if (hitInfo.collider.CompareTag("Obstacle")) { }
        }
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }
}
