using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeacherShadowController : MonoBehaviour
{
    private bool _canRotated = false;
    private bool _isMove = false;
    private bool _executeOnce = false;//一回だけ実行する
    private bool _rotationDirection = true;
    private bool _duringEvent = false;
    private float _time;
    private float _targetAngle = 360.0f - 108.0f;//-108だとMathf.Approximatelyが動かない！！
    private float _startAngle = 360.0f - 180.0f;
    private float _moveDistance = 0.1f;//上下移動の距離
    private float _moveSpeed = 5.0f;//移動速度
    private float _rotatedSpeed = 10.0f;
    private float _startAlpha;
    private float _SecondsToDoor = 6.0f;
    private float _teacherEventTime = 5.0f;
    private Vector3 _startPosition;
    private Renderer _teacherRenderer;
    [SerializeField] private DoorController _doorController;

    // Start is called before the first frame update
    private void Start()
    {
        _startPosition = transform.position;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, _startAngle);
        _teacherRenderer = GetComponent<Renderer>();
        _startAlpha = _teacherRenderer.material.color.a;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Init();
        }
        if (Mathf.Approximately(transform.eulerAngles.z, _startAngle) && !_rotationDirection)
        {
            transform.position = _startPosition;
            _canRotated = false;
            _duringEvent = false;
        }
        if (Mathf.Approximately(transform.eulerAngles.z, _targetAngle))
        {
            if (!_executeOnce)//一回だけ実行
            {
                _executeOnce = true;
                _isMove = true;
                StartCoroutine(MovePauseCoroutine(_SecondsToDoor));//ドアの前で止め、先生イベントを呼び出す
                StartCoroutine(FadeUpdateCoroutine());//透明にして元に戻す
            }
        }
        if (_isMove)
        {
            Move();
            MoveUpDown();
        }
        if (_canRotated)
        {
            Rotate(_rotationDirection);
        }
    }
    public void Init()
    {
        if (!_duringEvent)
        {
            _duringEvent = true;
            _time = 0.0f;
            _canRotated = true;
            _executeOnce = false;
            _isMove = false;
            _rotationDirection = true;
            transform.position = _startPosition;
        }
    }

    private void MoveUpDown()
    {
        _time += Time.deltaTime * _moveSpeed;

        float newY = Mathf.Sin(_time) * _moveDistance;
        transform.position = new Vector3(transform.position.x, _startPosition.y + newY, transform.position.z);
    }

    private void Rotate(bool rotationDirection)
    {
        if (rotationDirection)//出現
        {
            Quaternion targetRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, _targetAngle);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotatedSpeed * Time.deltaTime);
        }
        else//消失
        {
            Quaternion targetRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, _startAngle);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotatedSpeed * Time.deltaTime);
        }
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
    private void TeacherEvent()
    {

    }
    private IEnumerator MovePauseCoroutine(float second)
    {
        yield return new WaitForSeconds(second);
        _isMove = false;
        _doorController.IsOpen = true;
        TeacherEvent();
        yield return new WaitForSeconds(_teacherEventTime);//先生イベントの時間
        _doorController.IsOpen = false;
        _isMove = true;
        yield return new WaitForSeconds(6.0f);
        _isMove = false;
        _rotationDirection = false;
    }
    private IEnumerator FadeOutCoroutine()
    {
        float changeTime = 1.5f;
        float timeElapsed = 0.0f;

        while (timeElapsed < changeTime)
        {
            float alpha = Mathf.Lerp(_startAlpha, 0.0f, timeElapsed / changeTime);//透明度を滑らかに更新
            TransparencyUpdate(alpha);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        TransparencyUpdate(0.0f);
    }
    private IEnumerator FadeInCoroutine()
    {
        float changeTime = 1.0f;
        float timeElapsed = 0.0f;

        while (timeElapsed < changeTime)
        {
            float alpha = Mathf.Lerp(0.0f, _startAlpha, timeElapsed / changeTime);//透明度を滑らかに更新
            TransparencyUpdate(alpha);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        TransparencyUpdate(_startAlpha);
    }
    private IEnumerator FadeUpdateCoroutine()
    {
        yield return new WaitForSeconds(2.0f);
        StartCoroutine(FadeOutCoroutine());//透明にする
        yield return new WaitForSeconds(_SecondsToDoor - 2.0f);//透明にする前に待った2秒を引く
        yield return new WaitForSeconds(_teacherEventTime);
        yield return new WaitForSeconds(2.2f);
        StartCoroutine(FadeInCoroutine());//透明度を戻す
    }
}