using UnityEngine;
using UnityEngine.AI;
using Utils;

public class Enemy : CharacterController
{
    [SerializeField]
    private LayerMask _lookLayer;

    [SerializeField]
    private Player _player;

    [SerializeField]
    private bool drawPath;

    [SerializeField]
    private float _shotDistance;

    [SerializeField]
    private uint _playerCheckReflectionsCount = 3;

    [SerializeField]
    private bool drawLook;

    private NavMeshAgent _agent;

    private LineRenderer _lookRenderer;
    private LineRenderer _pathRenderer;
    private float _rotation;

    new void Start()
    {
        base.Start();

        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;

        _pathRenderer = CreateAndConfigureLineRenderer("Path Renderer", _color);
        _lookRenderer = CreateAndConfigureLineRenderer("Look Renderer", Color.red, 0.05f);

        Revive();
    }

    void Update()
    {
        if (Vector2.Distance(transform.position, _player.transform.position) <= _agent.stoppingDistance)
        {
            _rotation = AngleUtils.Angle(transform, _player.transform);
        }

        if (CheckPlayerIsVisible())
        {
            _rotation = AngleUtils.Angle(transform, _player.transform);

            Shoot();
        }
        else
        {
            _agent.SetDestination(_player.transform.position);
            _rotation = Vector2.SignedAngle(Vector2.right, _agent.velocity);

            DrawCurrentPath();
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
        Destroy(_lookRenderer.material);
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
                distance,
                _lookLayer
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

    void DrawCurrentPath()
    {
        if (drawPath)
        {
            _pathRenderer.positionCount = _agent.path.corners.Length;
            for (int i = 0; i < _agent.path.corners.Length; i++)
            {
                _pathRenderer.SetPosition(i, _agent.path.corners[i]);
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
