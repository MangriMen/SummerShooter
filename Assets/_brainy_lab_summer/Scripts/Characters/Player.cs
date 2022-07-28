using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class Player : CharacterController
{
    [SerializeField]
    private TextMeshProUGUI actionsText;

    private float _rotation;

    private float moveForwardCheckDelay = 0.1f;
    private float moveForwardCheckTimer = 0.1f;

    private float moveSidewaysCheckDelay = 0.1f;
    private float moveSidewaysCheckTimer = 0.1f;

    private float rotationCheckDelay = 0.4f;
    private float rotationCheckTimer = 0.4f;

    private float shotCheckDelay = 0.5f;
    private float shotCheckTimer = 0.5f;

    void Update()
    {
        _velocity = transform.right * Input.GetAxisRaw("Vertical") * _speed;
        _rotation = -Input.GetAxisRaw("Horizontal") * _rotationSpeed;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _gun.Shoot();

            CheckAction();

            shotCheckTimer = shotCheckDelay;
        }

        if (Input.GetAxisRaw("Vertical") != 0)
        {
            moveForwardCheckTimer = moveForwardCheckDelay;
        }

        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            rotationCheckTimer = rotationCheckDelay;
        }

        if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.E))
        {
            _velocity = (transform.right * Input.GetAxisRaw("Vertical")) + (transform.up * GetSidewayMovementDirection());
            _velocity = _velocity.normalized * _speed;

            moveSidewaysCheckTimer = moveSidewaysCheckDelay;
        }

        if (moveForwardCheckTimer > 0)
        {
            moveForwardCheckTimer -= Time.deltaTime;
        }

        if (moveSidewaysCheckTimer > 0)
        {
            moveSidewaysCheckTimer -= Time.deltaTime;
        }

        if (rotationCheckTimer > 0)
        {
            rotationCheckTimer -= Time.deltaTime;
        }

        if (shotCheckTimer > 0)
        {
            shotCheckTimer -= Time.deltaTime;
        }
    }

    new void FixedUpdate()
    {
        base.FixedUpdate();
        _rb.MoveRotation(_rb.rotation + _rotation * Time.deltaTime);
    }

    public void GetShot()
    {
        Kill();
    }

    int GetSidewayMovementDirection()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            return 1;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            return -1;
        }

        return 0;
    }

    void CheckAction()
    {
        if (moveForwardCheckTimer > 0 && rotationCheckTimer > 0 && shotCheckTimer > 0)
        {
            AppendAction("Movement, rotation and double shot");
        }
        else if (moveForwardCheckTimer > 0 && rotationCheckTimer > 0)
        {
            AppendAction("Movement, rotation and shot");
        }
        else if (moveSidewaysCheckTimer > 0 && shotCheckTimer > 0)
        {
            AppendAction("Sideways movement and double shot");
        }
        else if (moveForwardCheckTimer > 0 && shotCheckTimer > 0)
        {
            AppendAction("Movement and double shot");
        }
        else if (rotationCheckTimer > 0 && shotCheckTimer > 0)
        {
            AppendAction("Rotation and double shot");
        }
        else if (moveForwardCheckTimer > 0)
        {
            AppendAction("Movement and shot");
        }
        else if (rotationCheckTimer > 0)
        {
            AppendAction("Rotation and shot");
        }
        else if (moveSidewaysCheckTimer > 0)
        {
            AppendAction("Sideways movement and shot");
        }
        else if (shotCheckTimer > 0)
        {
            AppendAction("Double shot");
        }
    }

    void AppendAction(string action)
    {
        var newActionsText = actionsText.text.ToString();
        var actions = newActionsText.Split("\n");

        StringBuilder sb = new($"{action}\n");
        var actionsCount = actions.Length - (actions.Length >= 4 ? 1 : 0);
        for (int i = 0; i < actionsCount; i++)
        {
            sb.Append($"{actions[i]}\n");
        }
        sb.Remove(sb.Length - 1, 1);

        actionsText.SetText(sb.ToString());
    }
}
