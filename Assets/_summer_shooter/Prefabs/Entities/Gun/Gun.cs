using System.Threading.Tasks;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField]
    private GameObject _bullet;

    [SerializeField]
    private Transform _shotPoint;

    [SerializeField]
    private float _shotDelay;

    private float _shotTimer;

    public Transform ShotPoint { get => _shotPoint; }
    public Color Color { get => GetComponent<SpriteRenderer>().color; }

    void Update()
    {
        if (_shotTimer > 0)
        {
            _shotTimer -= Time.deltaTime;
        }
    }

    public async void Shoot(uint count = 1, float delay = 0)
    {
        if (_shotTimer <= 0)
        {
            for (uint i = 0; i < count; i++)
            {
                CreateBullet();
                await Task.Delay((int)(delay * 1000));
            }

            _shotTimer = _shotDelay;
        }
    }

    private GameObject CreateBullet()
    {
        GameObject go = Instantiate(_bullet, _shotPoint.position, transform.rotation);
        go.SendMessage("SetColor", Color);
        go.SendMessage("SetOwner", transform.parent.gameObject);
        return go;
    }

    public void SetColor(Color color)
    {
        GetComponent<SpriteRenderer>().color = color;
    }
}
