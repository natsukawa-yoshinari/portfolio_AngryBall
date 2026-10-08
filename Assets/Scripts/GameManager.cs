using LitMotion;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GoalPost goalPost;
    [SerializeField]
    private GameObject _startBackground;
    [SerializeField]
    private GameObject _endBackground;
    [SerializeField]
    private PlayerManager _playerManager;

    [SerializeField]
    private TextMeshProUGUI _scoreText;
    [SerializeField]
    private TextMeshProUGUI _resultText;
    [SerializeField]
    private TextMeshProUGUI _remainPlayerText;

    private ReactiveProperty<int> _currentScore = new(0);

    private void Start()
    {
        goalPost.OnGoalEvent += OnGoal;

        _currentScore.Pairwise()
        .Subscribe(score =>
        {
            LMotion.Create(score.Previous, score.Current, 0.5f)
                    .Bind(x => _scoreText.text = x.ToString("0000"));
        }).AddTo(this);

        _playerManager.remainPlayer.Where(x => x >= 0)
        .Subscribe(x =>
        {
            _remainPlayerText.text = x.ToString();
        });

        _playerManager.remainPlayer.Where(x => x <= 0)
        .Subscribe(x =>
        {
            _playerManager.isPlayerRemain = false;
        });

        _playerManager.stopPlayer.Where(x => x >= 3)
        .Subscribe(x =>
        {
            EndGame();
        });
    }

    private void OnEnable()
    {
        Obstacle.OnObstacleDestroyed += AddScore;
    }

    private void OnDisable()
    {
        Obstacle.OnObstacleDestroyed -= AddScore;
    }

    public void AddScore(int score)
    {
        _currentScore.Value += score;
    }

    private void OnGoal()
    {
        AddScore(1000);
        EndGame();
    }

    private void EndGame()
    {
        _endBackground.SetActive(true);
        _playerManager.DisableInput();

        LMotion.Create(0, _currentScore.Value, 1f)
                .Bind(x => _resultText.text = x.ToString("00000"))
                .AddTo(this);
    }

    public void ButtonStartGame()
    {
        _startBackground.SetActive(false);
        _playerManager.EnableInput();
    }

    public void ButtonResetGame()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}
