using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> _players;
    [SerializeField] private List<GameObject> _hutons;
    [SerializeField] private GameObject _door;
    [SerializeField] private GameObject _teacher;
    [SerializeField] private GameObject _meteor;
    [SerializeField] private GameObject _tatami;
    [SerializeField] private GameObject _makuraPrefub;
    [SerializeField] private GameObject _happeningBall;

    private List<GameObject> _makuras = new List<GameObject>();
    private List<MakuraController> _makuraControllers = new List<MakuraController>();
    private List<PlayerController> _playerControllers = new List<PlayerController>();
    private DoorController _doorController;
    private TeacherShadowController _teacherEvent;
    private MeteorEvent _meteorEvent;
    private TatamiEvent _tatamiEvent;
    private bool _isGameStart = false;
    private bool _isPlayerSet = false;
    private Vector3 initialPosition = new Vector3(-3.0f, 0.5f, 0.0f);

    private Event _event;
    private List<HappeningBall> _happeningBalls = new List<HappeningBall>();
    private bool aa = false;

    // Start is called before the first frame update
    private void Awake()
    {
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
            if (_happeningBall != null)
            {
                StartCoroutine(HappeningBallGeneration());
            }
        }
        if (_makuras != null)
        {
            foreach (var makura in _makuras)
            {
                var makuraController = makura.GetComponent<MakuraController>();
                _makuraControllers.Add(makuraController);
            }
        }
        if (_players != null)
        {
            foreach (var player in _players)
            {
                var playerController = player.GetComponent<PlayerController>();
                _playerControllers.Add(playerController);
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
        if (_happeningBalls != null)
        {
            foreach (var happeningBall in _happeningBalls)
            {
                if (happeningBall.Outbreak)
                {
                    _event.RandomEvent(happeningBall.Starter);
                    // happeningBall.Outbreak = false;
                }
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
        yield return new WaitForSeconds(30.0f);
        StartCoroutine(HappeningBallGeneration());
    }
}
