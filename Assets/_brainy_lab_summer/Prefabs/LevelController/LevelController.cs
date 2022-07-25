using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Utils;

public class LevelController : MonoBehaviour
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

        playerScore.color = player.color;
        enemyScore.color = enemy.color;

        RestartRound();
    }

    void Update()
    {
        if (needToRestartRound)
        {
            if (restartTimer <= 0)
            {
                RestartRound();
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

                win.SetText(enemy.nickname);
                win.color = enemy.color;

                needToRestartRound = true;
            }

            if (!enemy.IsAlive)
            {
                playerScore.SetText((int.Parse(playerScore.text.ToString()) + 1).ToString());

                win.SetText(player.nickname);
                win.color = player.color;

                needToRestartRound = true;
            }
        }
    }

    void RestartRound()
    {
        needToRestartRound = false;
        restartTimer = restartDelay;
        win.SetText("");

        var objects = FindObjectsOfType(typeof(GameObject));
        foreach (GameObject obj in objects)
        {
            if (obj.CompareTag("Bullet"))
            {
                Destroy(obj);
            }
        }

        player.Revive();
        enemy.Revive();

        player.transform.position = playerStartPos.transform.position;
        enemy.transform.position = enemyStartPos.transform.position;

        player.transform.rotation = Quaternion.Euler(
            player.transform.rotation.eulerAngles.x,
            player.transform.rotation.eulerAngles.y,
            TransformUtils.Angle(enemy.transform, player.transform)
        );

        enemy.transform.rotation = Quaternion.Euler(
            enemy.transform.rotation.eulerAngles.x,
            enemy.transform.rotation.eulerAngles.y,
            TransformUtils.Angle(player.transform, enemy.transform)
        );
    }
}
