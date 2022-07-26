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

    private float rotation;

    private Pathfinder pathfinder;
    private int currentPointIndex = -1;
    private Vector2[] currentPath;

    private LineRenderer lr;

    [SerializeField]
    private float pathUpdateDelay = 4f;
    private float pathUpdateTimer;

    private Transform prevStuckPosition;

    [SerializeField]
    bool drawPath;

    new void Start()
    {
        base.Start();

        gameObject.AddComponent<LineRenderer>();
        lr = GetComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
        lr.startWidth = lr.endWidth = 0.2f;
        lr.startColor = lr.endColor = color;
        lr.sortingOrder = -100;

        pathfinder = new(levelController.enemyPathMesh);

        Revive();
    }

    private void Update()
    {
        if (CheckPlayer())
        {
            velocity = Vector2.zero;
            rotation = TransformUtils.Angle(levelController.player.transform, transform);
            TryToShoot();
        }
        else
        {
            UpdatePathIfNeed();

            DrawCurrentPath();

            MoveToNextPathPoint();
        }

        if (pathUpdateTimer > 0)
        {
            pathUpdateTimer -= Time.deltaTime;
        }
    }

    new void FixedUpdate()
    {
        base.FixedUpdate();
        rb.MoveRotation(
            Quaternion.RotateTowards(
                Quaternion.Euler(0, 0, rb.rotation),
                Quaternion.Euler(0, 0, rotation),
                rotationAngle * Time.deltaTime
                ));
    }

    private void UpdatePathIfNeed()
    {
        if (pathUpdateTimer < 0 || currentPointIndex == -1)
        {
            if (pathfinder != null)
            {
                currentPath = pathfinder.FindPath(
                    transform.position,
                    levelController.player.transform.position, gameObject);

                currentPointIndex = 0;
                pathUpdateTimer = pathUpdateDelay;
            }
        }
    }

    private void DrawCurrentPath()
    {
        if (currentPath != null && drawPath)
        {
            lr.positionCount = currentPath.Length;
            for (int i = 0; i < currentPath.Length; i++)
            {
                lr.SetPosition(i, currentPath[i]);
            }
        }
    }

    private void MoveToNextPathPoint()
    {
        if (currentPath != null)
        {
            if (currentPointIndex >= currentPath.Length - 1)
            {
                currentPointIndex = -1;
                return;
            }

            if (Vector2.Distance(transform.position, currentPath[currentPointIndex]) < 0.5f)
            {
                velocity = Vector2.zero;
                currentPointIndex++;
            }
            else
            {
                var needRotation = TransformUtils.Angle(currentPath[currentPointIndex], transform.position);
                var angleDiff = Mathf.Abs(needRotation - rb.rotation);

                if (MathExtension.IsBetweenRange(angleDiff, 1, 359) || angleDiff > 361)
                {
                    rotation = needRotation;
                    velocity = Vector2.zero;
                }
                else
                {
                    velocity = transform.right * speed;
                }
            }
        }
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
            gun.Shoot(2, 0.15f);
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

    new private void Revive()
    {
        base.Revive();
        currentPointIndex = -1;
        pathUpdateTimer = pathUpdateDelay;
    }

    void OnDestroy()
    {
        Destroy(lr.material);
    }
}
