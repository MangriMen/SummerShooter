using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Level : MonoBehaviour
{
    public Player player;
    public Enemy enemy;

    public TextMeshProUGUI playerScore;
    public TextMeshProUGUI enemyScore;
    public TextMeshProUGUI win;

    [SerializeField]
    Transform playerStartPos;

    [SerializeField]
    Transform enemyStartPos;

    private bool needToRestartRound;

    [SerializeField]
    private float restartDelay = 2;
    private float restartTimer;

    void Start()
    {
        restartTimer = restartDelay;
        RestartRound();
    }

    void Update()
    {
        if (needToRestartRound)
        {
            if (restartTimer <= 0)
            {
                RestartRound();
                needToRestartRound = false;
                restartTimer = restartDelay;
                win.SetText("");
            }
            else
            {
                restartTimer -= Time.deltaTime;
            }
        }
        else
        {
            if (!player.IsAlive)
            {
                enemyScore.SetText((int.Parse(enemyScore.text.ToString()) + 1).ToString());

                win.SetText("BLUE");
                win.color = Color.blue;

                needToRestartRound = true;
            }

            if (!enemy.IsAlive)
            {
                playerScore.SetText((int.Parse(playerScore.text.ToString()) + 1).ToString());

                win.SetText("RED");
                win.color = Color.red;

                needToRestartRound = true;
            }
        }
    }

    void RestartRound()
    {
        player.Revive();
        enemy.Revive();

        player.transform.position = playerStartPos.transform.position;
        enemy.transform.position = enemyStartPos.transform.position;

        player.transform.rotation = Quaternion.Euler(
            player.transform.rotation.eulerAngles.x,
            player.transform.rotation.eulerAngles.y,
            GetLookAngle(enemy.transform, player.transform)
        );

        enemy.transform.rotation = Quaternion.Euler(
            enemy.transform.rotation.eulerAngles.x,
            enemy.transform.rotation.eulerAngles.y,
            GetLookAngle(player.transform, enemy.transform)
        );
    }

    public float GetLookAngle(Transform pointA, Transform pointB) =>
        Mathf.Atan2(
            pointA.transform.position.y - pointB.transform.position.y,
            pointA.transform.position.x - pointB.transform.position.x
        ) * Mathf.Rad2Deg;
}
