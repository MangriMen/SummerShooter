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
    private bool drawPath;

    [SerializeField]
    private float _shotDistance;

    [SerializeField]
    private uint _playerCheckReflectionsCount = 3;

    [SerializeField]
    private bool drawLook;

    private LineRenderer _lookRenderer;
    private LineRenderer _pathRenderer;
    private Pathfinder _pathfinder;
    private Vector2[] _currentPath;
    private int _currentPathPointIndex = -1;
    private float _pathUpdateTimer;
    private float _rotation;

    new void Start()
    {
        base.Start();

        _pathRenderer = CreateAndConfigureLineRenderer("Path Renderer", _color);
        _lookRenderer = CreateAndConfigureLineRenderer("Look Renderer", Color.red, 0.05f);

        _pathfinder = new(_enemyPathMesh);

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
        Destroy(_pathRenderer.material);
    }

    new void Revive()
    {
        base.Revive();
        _currentPathPointIndex = -1;
        _pathUpdateTimer = _pathUpdatePeriod;
    }

    LineRenderer CreateAndConfigureLineRenderer(string objectName, Color color, float width = 0.2f)
    {
        var childForLR = new GameObject(objectName);
        childForLR.transform.SetParent(transform);
        childForLR.transform.position = transform.position;
        childForLR.transform.rotation = transform.rotation;
        childForLR.AddComponent<LineRenderer>();

        var lr = childForLR.GetComponent<LineRenderer>();
        ConfigureLineRenderer(lr, color, width);

        return lr;
    }

    void ConfigureLineRenderer(LineRenderer lr, Color color, float width = 0.2f)
    {
        lr.material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
        lr.startWidth = lr.endWidth = width;
        lr.startColor = lr.endColor = color;
        lr.sortingOrder = -100;
    }

    bool CheckPlayerIsVisible()
    {
        var reflectionsCount = 0;

        var position = _gun.ShotPoint.transform.position;
        var direction = _gun.ShotPoint.transform.up;
        var distance = _shotDistance;

        _lookRenderer.positionCount = 0;

        do
        {
            var hitInfo = Physics2D.Raycast(
                position,
                direction,
                distance
            );

            if (drawLook)
            {
                _lookRenderer.positionCount += 1;
                _lookRenderer.SetPosition(reflectionsCount, position);
            }

            if (hitInfo.collider != null)
            {

                if (hitInfo.collider.CompareTag("Obstacle"))
                {
                    position = hitInfo.point;
                    direction = Vector2.Reflect(direction, hitInfo.normal).normalized;
                }
                else if (hitInfo.collider.CompareTag("Player"))
                {
                    return true;
                }
            }

            reflectionsCount++;
        } while (reflectionsCount <= _playerCheckReflectionsCount);

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
            _pathRenderer.positionCount = _currentPath.Length;
            for (int i = 0; i < _currentPath.Length; i++)
            {
                _pathRenderer.SetPosition(i, _currentPath[i]);
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
                var angleDiff = Mathf.Abs(needRotation - _rb.rotation) % 360;

                if (angleDiff.IsBetweenRange(30, 330))
                {
                    _rotation = needRotation;
                    _velocity = transform.right.normalized * _speed / 4;
                }
                else
                {
                    _velocity = transform.right.normalized * _speed;
                }
            }
        }
    }

    void Shoot()
    {
        if (Random.Range(0, 100) < 15)
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

    public void TogglePathTrace()
    {
        _pathRenderer.positionCount = 0;
        drawPath = !drawPath;
    }

    public void ToggleLookTrace()
    {
        _lookRenderer.positionCount = 0;
        drawLook = !drawLook;
    }
}
