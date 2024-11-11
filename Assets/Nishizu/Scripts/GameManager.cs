using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> _makuras;
    [SerializeField] private List<GameObject> _players;
    [SerializeField] private GameObject _door;
    [SerializeField] private GameObject _teacher;
    [SerializeField] private GameObject _meteor;
    [SerializeField] private GameObject _tatami;

    private List<MakuraController> _makuraControllers = new List<MakuraController>();
    private List<PlayerController> _playerControllers = new List<PlayerController>();
    private DoorController _doorController;
    private TeacherShadowController _teacherEvent;
    private MeteorEvent _meteorEvent;
    private TatamiEvent _tatamiEvent;
    private bool _isGameStart = false;
    private bool _isPlayerSet = false;
    private Vector3 initialPosition = new Vector3(-3.0f, 0.5f, 0.0f);


    // Start is called before the first frame update
    private void Start()
    {
        foreach (var makura in _makuras)
        {
            var makuraController = makura.GetComponent<MakuraController>();
            if (makuraController != null)
            {
                _makuraControllers.Add(makuraController);
            }
        }
        foreach (var player in _players)
        {
            var playerController = player.GetComponent<PlayerController>();
            if (playerController != null)
            {
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



        // _isGameStart = true;



    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            _isGameStart = true;
        }
        if (_isGameStart)
        {
            if (!_isPlayerSet)
            {
                for (int i = 0; i < _players.Count; i++)
                {
                    _players[i].transform.position = initialPosition;
                    initialPosition.x += 2.0f;
                    if (i == _players.Count - 1)
                    {
                        _isPlayerSet = true;
                    }
                }
            }



            //デバッグ用
            if (Input.GetKeyDown(KeyCode.S))
            {
                _teacherEvent.Init();
            }
            if (Input.GetKeyDown(KeyCode.T))
            {
                _tatamiEvent.Init();
            }
            if (Input.GetKeyDown(KeyCode.M))
            {
                _meteorEvent.Init();
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                foreach (var makura in _makuraControllers)
                {
                    makura.CurrentColorType = ColorChanger.ColorType.Red;
                }
            }
            if (Input.GetKeyDown(KeyCode.G))
            {
                foreach (var makura in _makuraControllers)
                {
                    makura.CurrentColorType = ColorChanger.ColorType.Green;
                }
            }
            if (Input.GetKeyDown(KeyCode.B))
            {
                foreach (var makura in _makuraControllers)
                {
                    makura.CurrentColorType = ColorChanger.ColorType.Blue;
                }
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                foreach (var makura in _makuraControllers)
                {
                    makura.CurrentColorType = ColorChanger.ColorType.Black;
                }
            }
            if (Input.GetKeyDown(KeyCode.N))
            {
                foreach (var makura in _makuraControllers)
                {
                    makura.CurrentColorType = ColorChanger.ColorType.Nomal;
                }
            }
        }
    }
    private void LateUpdate()
    {

    }

    private void Init()
    {

    }
}
