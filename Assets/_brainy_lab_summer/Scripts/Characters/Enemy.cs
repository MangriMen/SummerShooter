using UnityEngine;
using Utils;

public class Enemy : CharacterController
{
    [SerializeField]
    private Player _player;

    [SerializeField]
    private AINavMeshGenerator _enemyPathMesh;

    [SerializeField]
    private float _pathUpdatePeriod = 1f;

    [SerializeField]
    bool drawPath;

    [SerializeField]
    private float _shotDistance;

    private LineRenderer _lr;
    private Pathfinder _pathfinder;
    private Vector2[] _currentPath;
    private int _currentPathPointIndex = -1;
    private float _pathUpdateTimer;
    private float _rotation;

    new void Start()
    {
        base.Start();

        gameObject.AddComponent<LineRenderer>();
        _lr = GetComponent<LineRenderer>();
        _pathfinder = new(_enemyPathMesh);

        ConfigurePathDrawer();
        Revive();
    }

    void Update()
    {
        if (CheckPlayerIsVisible())
        {
            _velocity = Vector2.zero;
            _rotation = AngleUtils.Angle(transform, _player.transform);
            Shoot();
        }
        else
        {
            UpdatePathIfNeed();
            DrawCurrentPath();
            MoveToNextPathPoint();
        }

        if (_pathUpdateTimer > 0)
        {
            _pathUpdateTimer -= Time.deltaTime;
        }
    }

    new void FixedUpdate()
    {
        base.FixedUpdate();
        _rb.MoveRotation(
            Quaternion.RotateTowards(
                Quaternion.Euler(0, 0, _rb.rotation),
                Quaternion.Euler(0, 0, _rotation),
                _rotationSpeed * Time.deltaTime
                ));
    }

    void OnDestroy()
    {
        Destroy(_lr.material);
    }

    new void Revive()
    {
        base.Revive();
        _currentPathPointIndex = -1;
        _pathUpdateTimer = _pathUpdatePeriod;
    }

    void ConfigurePathDrawer()
    {
        _lr.material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
        _lr.startWidth = _lr.endWidth = 0.2f;
        _lr.startColor = _lr.endColor = _color;
        _lr.sortingOrder = -100;
    }

    bool CheckPlayerIsVisible()
    {
        var hitInfo = Physics2D.Raycast(
            _gun.ShotPoint.transform.position,
            _gun.ShotPoint.transform.up,
            _shotDistance
        );

        if (hitInfo.collider != null)
        {
            return hitInfo.collider.CompareTag("Player");
        }

        return false;
    }

    void UpdatePathIfNeed()
    {
        if (_pathUpdateTimer < 0 || _currentPathPointIndex == -1)
        {
            _currentPath = _pathfinder.FindPath(
                transform.position,
                _player.transform.position, gameObject);

            _currentPathPointIndex = 0;
            _pathUpdateTimer = _pathUpdatePeriod;
        }
    }

    void DrawCurrentPath()
    {
        if (_currentPath != null && drawPath)
        {
            _lr.positionCount = _currentPath.Length;
            for (int i = 0; i < _currentPath.Length; i++)
            {
                _lr.SetPosition(i, _currentPath[i]);
            }
        }
    }

    void MoveToNextPathPoint()
    {
        if (_currentPath != null)
        {
            if (_currentPathPointIndex >= _currentPath.Length - 1)
            {
                _currentPathPointIndex = -1;
                return;
            }

            if (Vector2.Distance(transform.position, _currentPath[_currentPathPointIndex]) < 0.5f)
            {
                _velocity = Vector2.zero;
                _currentPathPointIndex++;
            }
            else
            {
                var needRotation = AngleUtils.Angle(transform.position, _currentPath[_currentPathPointIndex]);
                var angleDiff = Mathf.Abs(needRotation - _rb.rotation);

                if (angleDiff.IsBetweenRange(1, 359) || angleDiff > 361)
                {
                    _rotation = needRotation;
                    _velocity = Vector2.zero;
                }
                else
                {
                    _velocity = transform.right * _speed;
                }
            }
        }
    }

    void Shoot()
    {
        if (Random.Range(1, 100) < 30)
        {
            _gun.Shoot(2, 0.15f);
        }
        else
        {
            _gun.Shoot();
        }
    }

    public void GetShot()
    {
        Kill();
    }
}
