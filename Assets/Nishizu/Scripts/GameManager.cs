using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> _players;
    [SerializeField] private List<GameObject> _hutons;
    [SerializeField] private GameObject _door;
    [SerializeField] private GameObject _teacher;
    [SerializeField] private GameObject _meteor;
    [SerializeField] private GameObject _tatami;
    [SerializeField] private GameObject _makuraPrefub;
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


    // Start is called before the first frame update
    private void Start()
    {
        if (_hutons != null)
        {
            foreach (var huton in _hutons)
            {
                Vector3 hutonPosition = huton.GetComponent<HutonController>().GetCenterPosition();
                Quaternion hutonRotation = huton.GetComponent<HutonController>().GetRotation();
                _makuras.Add(Instantiate(_makuraPrefub, new Vector3(hutonPosition.x, hutonPosition.y + 0.1f, hutonPosition.z + 0.6f), hutonRotation));
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
                    // Debug.Log($"Setting position for player {i} to {initialPosition}");
                    _players[i].transform.position = initialPosition;

                    // Debug.Log($"New position: {_players[i].transform.position}");
                    initialPosition.x += 2.0f;
                    // if (i == _players.Count - 1)
                    // {
                    //     _isPlayerSet = true;
                    // }
                }
                _isPlayerSet = true;
            }
        }



        //デバッグ用
        if (Input.GetKeyDown(KeyCode.T))
        {
            _teacherEvent.Init();
        }
        if (Input.GetKeyDown(KeyCode.F))
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
    // void LateUpdate()
    // {
    //     if (!_isPlayerSet)
    //     {
    // for (int i = 0; i<_players.Count; i++)
    // {
    //     // _players[i].transform.position = initialPosition;

    //     Debug.Log($"Setting position for player {i} to {initialPosition}");
    //     _players[i].transform.position = initialPosition;

    //     Debug.Log($"New position: {_players[i].transform.position}");
    //     initialPosition.x += 2.0f;
    //     if (i == _players.Count - 1)
    //     {
    //         _isPlayerSet = true;
    //     }
    // }

    // IEnumerator ForcePositionReset()
    // {
    //     yield return new WaitForEndOfFrame();  // 次のフレームに遅延

    //     for (int i = 0; i < _players.Count; i++)
    //     {
    //         if (_players[i] != null)
    //         {
    //             Debug.Log($"Setting position for player {i} to {initialPosition}");
    //             _players[i].transform.position = initialPosition;
    //             Debug.Log($"New position: {_players[i].transform.position}");

    //             initialPosition.x += 2.0f;  // 次のプレイヤーは x 座標が +2 になる
    //         }
    //     }

    //     _isPlayerSet = true;
    // }
    private void Init()
    {

    }
}
