using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    private List<GameObject> _players;
    [SerializeField] private List<GameObject> _hutons;
    [SerializeField] private GameObject _door;
    [SerializeField] private GameObject _teacher;
    [SerializeField] private GameObject _meteor;
    [SerializeField] private GameObject _tatami;
    [SerializeField] private GameObject _makuraPrefub;
    [SerializeField] private GameObject _happeningBall;
    [SerializeField] private GameObject _playerInputManager;
    private PlayerInputManager _playerInputM;
    private List<GameObject> _makuras = new List<GameObject>();
    private List<MakuraController> _makuraControllers = new List<MakuraController>();
    private List<PlayerController> _playerControllers = new List<PlayerController>();
    private DoorController _doorController;
    private TeacherShadowController _teacherEvent;
    private MeteorEvent _meteorEvent;
    private TatamiEvent _tatamiEvent;
    private bool _isGameStart = false;
    private bool _isPlayerSet = true;
    private Vector3 initialPosition = new Vector3(-3.0f, 0.5f, 0.0f);
    private Event _event;
    private List<HappeningBall> _happeningBalls = new List<HappeningBall>();

    // Start is called before the first frame update
    private void Awake()
    {
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
        if (_meteor != null)
        {
            _meteorEvent = _meteor.GetComponent<MeteorEvent>();
        }
        if (_tatami != null)
        {
            _tatamiEvent = _tatami.GetComponent<TatamiEvent>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_isGameStart)
        {
            if (_isPlayerSet)
            {
                for (int i = 0; i < _players.Count; i++)
                {
                    var playerController = _players[i].GetComponent<PlayerController>();
                    _playerControllers.Add(playerController);
                    _isPlayerSet = false;
                }
            }
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
        else
        {
            _players = _playerInputM.Players;
            _event.Players = _players;
        }
        if (_players.Count > 1 && Input.GetKeyDown(KeyCode.L))
        {
            _isGameStart = true;
            _event.IsGameStart = true;
            if (_happeningBall != null)
            {
                StartCoroutine(HappeningBallGeneration());
            }
        }

    }

    private void Init()
    {

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

    private IEnumerator HappeningBallGeneration()
    {
        GameObject happeningBall = Instantiate(_happeningBall, RandomPosition(), Quaternion.identity);
        _happeningBalls.Add(happeningBall.GetComponent<HappeningBall>());
        yield return new WaitForSeconds(10.0f);
        StartCoroutine(HappeningBallGeneration());
    }
}
