using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    private List<GameObject> _door = new List<GameObject>();
    private Vector3[] _closedPosition = new Vector3[2];
    private Vector3[] _openPosition = new Vector3[2];
    private float _openSpeed = 2.0f;
    private bool _isOpen = false;//ドアが開いているかどうか

    public bool IsOpen { get => _isOpen; set => _isOpen = value; }

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform)
        {
            _door.Add(child.gameObject);
        }
        _closedPosition[0] = _door[0].transform.position;
        _openPosition[0] = _closedPosition[0] + new Vector3(-0.6f, 0, 0);//左のドア
        _openPosition[0].y = _door[0].transform.position.y;

        _closedPosition[1] = _door[1].transform.position;
        _openPosition[1] = _closedPosition[1] + new Vector3(0.6f, 0, 0);//右のドア
        _openPosition[1].y = _door[1].transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        MoveDoors(_isOpen);
    }

    private void MoveDoors(bool isOpen)
    {
        Vector3 targetPositionLeft = isOpen ? _openPosition[0] : _closedPosition[0];
        Vector3 targetPositionRight = isOpen ? _openPosition[1] : _closedPosition[1];

        _door[0].transform.position = Vector3.Lerp(_door[0].transform.position, targetPositionLeft, _openSpeed * Time.deltaTime);
        _door[1].transform.position = Vector3.Lerp(_door[1].transform.position, targetPositionRight, _openSpeed * Time.deltaTime);
    }
}
