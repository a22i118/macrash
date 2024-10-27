using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeacherController : MonoBehaviour
{
    private float _moveDistance = 0.1f; // 上下移動の距離
    private float _moveSpeed = 5.0f; // 移動速度
    private float _rotatedSpeed = 10.0f;
    private Vector3 _startPosition;
    private float _time;
    private float _targetAngle = 360.0f - 108.0f;//-108だとMathf.Approximatelyが動かない！！
    private bool _canRotated = false;
    private Renderer _teacherRenderer;
    private bool _isMove = false;

    // Start is called before the first frame update
    private void Start()
    {
        _startPosition = transform.position;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, -180.0f); // 初期回転を-180度に設定
        _teacherRenderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    private void Update()
    {

        if (Mathf.Approximately(transform.eulerAngles.z, _targetAngle))
        {
            MoveUpDown();
            Move();
            if (!_isMove)
            {
                _isMove = true;
                StartCoroutine(FadeOutCoroutine(2.0f));
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _canRotated = true;
        }
        if (_canRotated)
        {
            Rotate();
        }
    }

    private void MoveUpDown()
    {
        _time += Time.deltaTime * _moveSpeed;

        float newY = Mathf.Sin(_time) * _moveDistance;
        transform.position = new Vector3(transform.position.x, _startPosition.y + newY, transform.position.z);
    }

    private void Rotate()
    {
        Quaternion targetRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, _targetAngle);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotatedSpeed * Time.deltaTime);
    }
    private void Move()
    {
        float newX = _startPosition.x + _time / 4.0f;
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }
    private void TransparencyUpdate(float alpha)
    {
        if (_teacherRenderer != null)
        {
            Color color = _teacherRenderer.material.color;
            color.a = alpha;//透明度
            _teacherRenderer.material.color = color;
        }
    }
    private IEnumerator FadeOutCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);

        float changeTime = 1.5f;
        float startAlpha = _teacherRenderer.material.color.a;
        float timeElapsed = 0.0f;

        while (timeElapsed < changeTime)
        {
            float alpha = Mathf.Lerp(startAlpha, 0.0f, timeElapsed / changeTime);//透明度を滑らかに更新
            TransparencyUpdate(alpha);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        TransparencyUpdate(0.0f);
    }
}
