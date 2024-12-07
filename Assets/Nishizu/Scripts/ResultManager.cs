using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
public class ResultManager : MonoBehaviour
{
    [SerializeField] private bool _isGameEnd = false;
    [SerializeField] private List<GameObject> _resultHutons = new List<GameObject>();
    [SerializeField] private List<GameObject> _resultScores = new List<GameObject>();
    private bool _isSceneSwith = false;
    private ResultCameraController _resultCameraController;
    private List<ResultHutonController> _resultHutonControllers = new List<ResultHutonController>();
    private Dictionary<int, int> _scoreDic = new Dictionary<int, int>();
    public bool IsGameEnd { get => _isGameEnd; set => _isGameEnd = value; }
    public List<ResultHutonController> ResultHutonControllers { get => _resultHutonControllers; set => _resultHutonControllers = value; }
    public Dictionary<int, int> ScoreDic { get => _scoreDic; set => _scoreDic = value; }

    // Start is called before the first frame update
    void Start()
    {
        foreach (var resultHuton in _resultHutons)
        {
            _resultHutonControllers.Add(resultHuton.GetComponent<ResultHutonController>());
        }
        foreach (var score in _resultScores)
        {
            score.SetActive(false);
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
        if (_resultCameraController.IsUISet)
        {
            for (int i = 0; i < _scoreDic.Count; i++)
            {
                _resultScores[i].SetActive(true);

                TextMeshProUGUI text = _resultScores[i].transform.GetChild(2).GetComponent<TextMeshProUGUI>();
                text.text = _scoreDic[i].ToString();
            }
            StartCoroutine(ScenesSwitch());
            _resultCameraController.IsUISet = false;
        }
        if (_isSceneSwith)
        {
            //シーンの切り替え
        }
    }
    private IEnumerator ScenesSwitch()
    {
        yield return new WaitForSeconds(5.0f);
        _isSceneSwith = true;
    }
}
