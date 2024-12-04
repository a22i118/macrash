using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> _resultHutons = new List<GameObject>();
    private List<ResultHutonController> _resultHutonControllers = new List<ResultHutonController>();
    private ResultCameraController _resultCameraController;
    private bool _isGameEnd = false;

    public bool IsGameEnd { get => _isGameEnd; set => _isGameEnd = value; }
    public List<ResultHutonController> ResultHutonControllers { get => _resultHutonControllers; set => _resultHutonControllers = value; }

    // Start is called before the first frame update
    void Start()
    {
        foreach (var resultHuton in _resultHutons)
        {
            _resultHutonControllers.Add(resultHuton.GetComponent<ResultHutonController>());
        }
        _resultCameraController = this.gameObject.GetComponent<ResultCameraController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_isGameEnd)
        {
            _resultCameraController.IsGameEnd = true;
        }
    }
}
