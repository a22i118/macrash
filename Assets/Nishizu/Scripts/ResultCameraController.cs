using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultCameraController : MonoBehaviour
{
    [SerializeField] private Camera _resultCamera;
    private bool _isGameEnd = false;
    private bool _isCameraSet = true;
    private bool _isCanMove = false;
    private bool _isUISet = false;
    private float _rotationSpeed = 75.0f;
    private float _moveSpeed = 0.035f;
    private float _targetPosition = 2.2f;
    private float _targetRotation = 180f;
    public bool IsGameEnd { get => _isGameEnd; set => _isGameEnd = value; }
    public bool IsUISet { get => _isUISet; set => _isUISet = value; }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        if (_isGameEnd)
        {
            if (_isCameraSet)
            {
                _isCameraSet = false;
                StartCoroutine(CameraSetDerey());
            }
            if (_isCanMove)
            {
                MoveCamera();
                RotateCamera();
            }
            if (_resultCamera.transform.position.y == 2.2f)
            {
                StartCoroutine(UISetDerey());
            }
        }
    }

    private void MoveCamera()
    {
        Vector3 currentPosition = _resultCamera.transform.position;
        _resultCamera.transform.position = Vector3.MoveTowards(_resultCamera.transform.position, new Vector3(currentPosition.x, _targetPosition, currentPosition.z), _moveSpeed);
    }

    private void RotateCamera()
    {
        Quaternion currentRotation = _resultCamera.transform.rotation;

        Quaternion targetRotation = Quaternion.Euler(90, _targetRotation, 0);

        _resultCamera.transform.rotation = Quaternion.RotateTowards(currentRotation, targetRotation, _rotationSpeed * Time.deltaTime);
    }
    private IEnumerator CameraSetDerey()
    {
        yield return new WaitForSeconds(0.5f);
        _isCanMove = true;
    }
    private IEnumerator UISetDerey()
    {
        yield return new WaitForSeconds(1.0f);
        _isUISet = true;
    }
}
