using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using System.Linq;

public class GameManager : MonoBehaviour
{
    private List<GameObject> _players;
    [SerializeField] private List<GameObject> _hutons;
    [SerializeField] private GameObject _door;
    [SerializeField] private GameObject _teacher;
    [SerializeField] private GameObject _makuraPrefub;
    [SerializeField] private GameObject _happeningBall;
    [SerializeField] private GameObject _playerInputManager;
    [SerializeField] private GameObject _scoreManager;
    [SerializeField] private GameObject _clock;
    [SerializeField] private Camera _resultCamera;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private GameObject _result;
    private ResultManager _resultManager;
    private PlayerInputManager _playerInputM;
    private List<GameObject> _makuras = new List<GameObject>();
    private List<MakuraController> _makuraControllers = new List<MakuraController>();
    private List<PlayerController> _playerControllers = new List<PlayerController>();
    private DoorController _doorController;
    private TeacherShadowController _teacherEvent;

    private bool _isGameStart = false;
    private bool _isPlayerSet = true;
    private bool _isGameStartCheck = false;
    private bool _isGameEnd = false;
    private Event _event;
    private List<HappeningBall> _happeningBalls = new List<HappeningBall>();

    // Start is called before the first frame update
    private void Awake()
    {
        _mainCamera.enabled = true;
        _resultCamera.enabled = false;
        _resultManager = _result.GetComponent<ResultManager>();
        _playerInputM = _playerInputManager.GetComponent<PlayerInputManager>();
        _event = GetComponent<Event>();

        if (_hutons != null)
        {
            foreach (var huton in _hutons)
            {
                Vector3 hutonPosition = huton.GetComponent<HutonController>().GetCenterPosition();
                Quaternion hutonRotation = huton.GetComponent<HutonController>().GetRotation();
                _makuras.Add(Instantiate(_makuraPrefub, new Vector3(hutonPosition.x, hutonPosition.y + 0.1f, hutonPosition.z + 0.6f), hutonRotation));
            }
            _event.Makuras = _makuras;
        }
        if (_makuras != null)
        {
            foreach (var makura in _makuras)
            {
                var makuraController = makura.GetComponent<MakuraController>();
                _makuraControllers.Add(makuraController);
            }
        }

        if (_door != null)
        {
            _doorController = _door.GetComponent<DoorController>();
        }
        if (_teacher != null)
        {
            _teacherEvent = _teacher.GetComponent<TeacherShadowController>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isGameEnd)
        {
            if (_isGameStart)
            {
                HappeningBallEvnt();
                if (_isPlayerSet)
                {
                    for (int i = 0; i < _players.Count; i++)
                    {
                        var playerController = _players[i].GetComponent<PlayerController>();
                        playerController.WakeUp();
                        _playerControllers.Add(playerController);

                    }
                    _isPlayerSet = false;
                }
            }
            else
            {
                _players = _playerInputM.Players;
                _event.Players = _players;
                foreach (var player in _players)
                {
                    //_players.Count > 1 &&
                    if (player.GetComponent<PlayerController>().IsGameStartCheck)
                    {
                        _isGameStartCheck = true;
                    }
                }

                if (SleepCheck(_players) && (_players.Count == 4 || _isGameStartCheck))
                {
                    Init();
                }
            }
        }
    }
    private void Init()
    {
        _isGameStart = true;
        _event.IsGameStart = true;
        _scoreManager.GetComponent<ScoreManager>().IsGameStart = true;
        _clock.GetComponent<ClockController>().IsGameStart = true;
        foreach (var player in _players)
        {
            player.GetComponent<PlayerStatus>().IsGameStart = true;
            player.GetComponent<PlayerController>().IsGameStart = true;
        }
        if (_happeningBall != null)
        {
            StartCoroutine(HappeningBallGeneration());
        }
        _playerInputManager.SetActive(false);
        StartCoroutine(GameEnd());
    }
    private bool SleepCheck(List<GameObject> players)
    {
        foreach (var player in players)
        {
            if (!player.GetComponent<PlayerController>().IsSleep)
            {
                return false;
            }
        }
        return true;
    }
    private Vector3 RandomPosition()
    {
        float xMin = -8.0f;
        float xMax = 8.0f;
        float zMin = -3.0f;
        float zMax = 7.0f;
        float y = 6.0f;

        float randomX = Random.Range(xMin, xMax);
        float randomZ = Random.Range(zMin, zMax);

        return new Vector3(randomX, y, randomZ);
    }
    private void HappeningBallEvnt()
    {
        if (_happeningBalls != null)
        {
            foreach (var happeningBall in _happeningBalls)
            {
                if (happeningBall.Outbreak)
                {
                    _event.RandomEvent(happeningBall.Starter);
                    happeningBall.Outbreak = false;
                }
            }
            if (_happeningBalls.Count > 10)
            {
                HappeningBall happeningBall = _happeningBalls[0];
                _happeningBalls.RemoveAt(0);
                if (happeningBall != null)
                {
                    Destroy(happeningBall.gameObject);
                }
            }
        }
    }
    private IEnumerator HappeningBallGeneration()
    {
        GameObject happeningBall = Instantiate(_happeningBall, RandomPosition(), Quaternion.identity);
        _happeningBalls.Add(happeningBall.GetComponent<HappeningBall>());
        yield return new WaitForSeconds(10.0f);
        if (_isGameStart)
        {
            StartCoroutine(HappeningBallGeneration());
        }
    }
    private IEnumerator GameEnd()
    {
        yield return new WaitForSeconds(120.0f);//6分360.0f
        _isGameStart = false;
        _isGameStartCheck = false;
        _event.IsGameStart = false;
        _scoreManager.GetComponent<ScoreManager>().IsGameStart = false;
        _clock.GetComponent<ClockController>().IsGameStart = false;
        foreach (var player in _players)
        {
            player.GetComponent<PlayerStatus>().IsGameStart = false;
        }
        int hutonIndex = 0;
        foreach (var playerController in _playerControllers)
        {
            playerController.IsGameStart = false;
            playerController.IsGameEnd = true;
            playerController.CurrentMakuraDisplay.SetActive(false);
            playerController.SpGageInstance.SetActive(false);
            playerController.ResultSleep(_resultManager.ResultHutonControllers[hutonIndex]);
            hutonIndex++;
        }
        for (int i = _happeningBalls.Count - 1; i >= 0; i--)
        {
            var happeningBall = _happeningBalls[i];

            if (happeningBall != null)
            {
                Destroy(happeningBall.gameObject);
                yield return null;
                _happeningBalls.RemoveAt(i);
            }
        }
        yield return new WaitForSeconds(5.0f);
        _mainCamera.enabled = false;
        _resultCamera.enabled = true;
        _resultManager.IsGameEnd = true;

        // SortScores(_scoreManager.GetComponent<ScoreManager>().ScoreNum);
        int scoretmp = -1;
        int rank = -1;
        int rankSkip = 0;
        foreach (var score in SortScores(_scoreManager.GetComponent<ScoreManager>().ScoreNum))
        {
            Debug.Log($"Player {score.Key}: {score.Value}");

            if (scoretmp != score.Value)
            {
                scoretmp = score.Value;
                rank += 1 + rankSkip;
                rankSkip = 0;
            }
            else
            {
                rankSkip++;
            }
            _resultManager.ResultHutonControllers[score.Key].Rank = rank;

        }
    }

    public static List<KeyValuePair<int, int>> SortScores(Dictionary<int, int> scoreDic)
    {
        return scoreDic.OrderByDescending(entry => entry.Value).ToList();
    }
}
