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

    private float moveCheckDelay = 0.1f;
    private float moveCheckTimer = 0.1f;

    private float rotationCheckDelay = 0.4f;
    private float rotationCheckTimer = 0.4f;

    private float shootCheckDelay = 0.5f;
    private float shootCheckTimer = 0.5f;

    void Update()
    {
        _velocity = transform.right * Input.GetAxisRaw("Vertical") * _speed;
        _rotation = -Input.GetAxisRaw("Horizontal") * _rotationSpeed;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _gun.Shoot();

            CheckAction();

            shootCheckTimer = shootCheckDelay;
        }

        if (Input.GetAxisRaw("Vertical") != 0)
        {
            moveCheckTimer = moveCheckDelay;
        }

        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            rotationCheckTimer = rotationCheckDelay;
        }

        if (moveCheckTimer > 0)
        {
            moveCheckTimer -= Time.deltaTime;
        }

        if (rotationCheckTimer > 0)
        {
            rotationCheckTimer -= Time.deltaTime;
        }

        if (shootCheckTimer > 0)
        {
            shootCheckTimer -= Time.deltaTime;
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

    void CheckAction()
    {
        if (moveCheckTimer > 0 && shootCheckTimer > 0)
        {
            AppendAction("Movement and double shot");
        }
        else if (rotationCheckTimer > 0 && shootCheckTimer > 0)
        {
            AppendAction("Rotation and double shot");
        }
        else if (rotationCheckTimer > 0)
        {
            AppendAction("Rotation and shot");
        }
        else if (moveCheckTimer > 0)
        {
            AppendAction("Movement and shot");
        }
        else if (shootCheckTimer > 0)
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
