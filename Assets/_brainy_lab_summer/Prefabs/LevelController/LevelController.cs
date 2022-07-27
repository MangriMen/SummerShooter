using TMPro;
using UnityEngine;
using Utils;

public class LevelController : MonoBehaviour
{
    [SerializeField]
    private Player _player;

    [SerializeField]
    private Transform _playerStartPos;

    [SerializeField]
    private TextMeshProUGUI _playerScore;

    [SerializeField]
    private Enemy _enemy;

    [SerializeField]
    private Transform _enemyStartPos;

    [SerializeField]
    private TextMeshProUGUI _enemyScore;

    [SerializeField]
    private AINavMeshGenerator _enemyPathMesh;

    [SerializeField]
    private TextMeshProUGUI _winText;

    [SerializeField]
    private float _roundRestartDelay = 2;

    private bool _needToRestartRound;
    private float _roundRestartTimer;

    public Player Player { get => _player; }
    public Enemy Enemy { get => _enemy; }
    public AINavMeshGenerator EnemyPathMesh { get => _enemyPathMesh; }

    void Start()
    {
        _playerScore.color = _player.Color;
        _enemyScore.color = _enemy.Color;

        RestartRound();
    }

    void Update()
    {
        if (_needToRestartRound)
        {
            if (_roundRestartTimer <= 0)
            {
                RestartRound();
            }
            else
            {
                _roundRestartTimer -= Time.deltaTime;
            }
        }
        else
        {
            if (!_player.IsAlive)
            {
                SetWin(_enemy, _enemyScore);
            }
            else if (!_enemy.IsAlive)
            {
                SetWin(_player, _playerScore);
            }
        }
    }

    void SetWin(CharacterController winningCharacter, TextMeshProUGUI score)
    {
        score.SetText((int.Parse(score.text.ToString()) + 1).ToString());

        _winText.SetText(winningCharacter.Name);
        _winText.color = winningCharacter.Color;

        _needToRestartRound = true;
    }

    void RestartRound()
    {
        ResetVariables();

        DestroyAllObjectsOnScene("Bullet");

        ResetCharacter(_player, _playerStartPos);
        ResetCharacter(_enemy, _enemyStartPos);
    }

    void ResetVariables()
    {
        _winText.SetText("");
        _roundRestartTimer = _roundRestartDelay;
        _needToRestartRound = false;
    }

    void ResetCharacter(CharacterController character, Transform startPosition)
    {
        character.Revive();
        character.transform.position = startPosition.transform.position;
        character.transform.rotation = Quaternion.Euler(0, 0, AngleUtils.AngleToDirectionAngle(
            AngleUtils.Angle(character.transform.position, new Vector2(0, 0)))
            );
    }

    void DestroyAllObjectsOnScene(string tag)
    {
        var objects = FindObjectsOfType(typeof(GameObject));
        foreach (GameObject obj in objects)
        {
            if (obj.CompareTag(tag))
            {
                Destroy(obj);
            }
        }
    }
}
