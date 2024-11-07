using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakuraController : ColorChanger
{
    [SerializeField] private LayerMask _groundLayer;
    private Rigidbody _rb;
    private Collider _col;
    private ScaleType _currentScaleType = ScaleType.Nomal;//今の大きさ
    private bool _isThrow = false;//投げられているかどうか
    private bool _isHitCoolTime = false;//当たった時のクールタイム
    private bool _isAlterEgo = false;//分身
    private bool _isTouching = false;//何かと接触しているか
    private Quaternion _initialRotation;//最初の向き
    private GameObject _thrower;//投げたプレイヤー
    private Vector3 _targetPosition;
    public bool IsThrow { get => _isThrow; set => _isThrow = value; }
    public GameObject Thrower { get => _thrower; set => _thrower = value; }
    public ScaleType CurrentScaleType { get => _currentScaleType; set => _currentScaleType = value; }
    public bool IsAlterEgo { get => _isAlterEgo; set => _isAlterEgo = value; }
    public enum ScaleType
    {
        Nomal,
        First,
        Second
    }
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _col = GetComponent<Collider>();
        _initialRotation = transform.rotation;
    }
    void Update()
    {
        ScaleChange(_currentScaleType);
        ColorChange(_currentColorType);
        BlackMakuraPositionUpdate();
        if (_isThrow)
        {
            StartCoroutine(AutoUseGrabity());
        }
        if (transform.position.y >= 9.5f)
        {
            _rb.useGravity = true;
        }
        if (_currentColorType == ColorType.Black && _isThrow)
        {
            StartCoroutine(BlackMakuraPlayerSerch());
            if (_rb.useGravity && !_isTouching)
            {
                _col.isTrigger = false;
            }
        }

        //デバッグ用
        if (Input.GetKeyDown(KeyCode.R))
        {
            _currentColorType = ColorType.Red;
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            _currentColorType = ColorType.Green;
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            _currentColorType = ColorType.Blue;
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            _currentColorType = ColorType.Black;
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            _currentColorType = ColorType.Nomal;
        }

    }
    private void ScaleChange(ScaleType type)
    {
        if (type == ScaleType.Second)
        {
            transform.localScale = new Vector3(6.0f, 6.0f, 6.0f);
        }
        else if (type == ScaleType.First)
        {
            transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
            StartCoroutine(ScaleChangeCoolTime());
        }
        else if (type == ScaleType.Nomal)
        {
            transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

        _isTouching = true;
        if (!_rb.isKinematic)
        {
            if (CurrentColorType != ColorType.Red && !_rb.useGravity && _rb.velocity != Vector3.zero && collision.gameObject != _thrower)
            {
                _rb.useGravity = true;
                _rb.velocity = Vector3.zero;
            }

            if (collision.gameObject.CompareTag("Player") && _isThrow && !_isHitCoolTime && collision.gameObject != _thrower)
            {

                _currentScaleType = ScaleType.Nomal;
                _rb.useGravity = true;
                _rb.velocity = Vector3.zero;
                Debug.Log("敵に当たったぜ");
                // StartCoroutine(HitCoolTime());
            }
            if (collision.gameObject.CompareTag("Makura"))
            {
                _isThrow = false;
                _currentScaleType = ScaleType.Nomal;
                _rb.useGravity = true;
                _rb.velocity = Vector3.zero;
                transform.rotation = Quaternion.Euler(_initialRotation.eulerAngles.x, transform.rotation.eulerAngles.y, _initialRotation.eulerAngles.z);
                if (_isThrow)
                {

                }
            }
        }

        if (((_groundLayer & (1 << collision.gameObject.layer)) == _groundLayer) || collision.gameObject.CompareTag("Huton"))
        {
            _isThrow = false;
            _currentScaleType = ScaleType.Nomal;
            transform.rotation = Quaternion.Euler(_initialRotation.eulerAngles.x, transform.rotation.eulerAngles.y, _initialRotation.eulerAngles.z);
            if (!_rb.isKinematic)
            {
                _rb.velocity = Vector3.zero;
            }
            _rb.isKinematic = true;
        }
        if (_isAlterEgo)
        {
            Destroy(gameObject);
        }


    }
    private Vector3 TargetPosition()
    {
        Vector3 playerPosition = transform.position;
        Collider[] hitColliders = Physics.OverlapSphere(playerPosition, 20.0f);

        foreach (var collider in hitColliders)
        {
            if (collider.CompareTag("Player") && collider.gameObject != gameObject)
            {
                return collider.gameObject.transform.position;
            }
        }
        return Vector3.zero;
    }
    private void OnTriggerEnter(Collider collider)
    {
        _isTouching = true;
        if (_col.isTrigger && _isThrow && !_isHitCoolTime && collider.gameObject != _thrower)
        {
            if (collider.gameObject.CompareTag("Player"))
            {
                _col.isTrigger = false;
                _isThrow = false;
                _currentScaleType = ScaleType.Nomal;
                _rb.useGravity = true;
                _rb.velocity = Vector3.zero;
            }
            if (collider.gameObject.CompareTag("Makura"))
            {
                Rigidbody rb = collider.gameObject.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.useGravity = true;
                    rb.velocity = Vector3.zero;
                }
            }
        }
    }
    private void OnTriggerExit()
    {
        _isTouching = false;
    }
    private void BlackMakuraPositionUpdate()
    {
        float xMin = -9f;
        float xMax = 9f;
        float yMin = 0f;
        float yMax = 6f;
        float zMin = -4f;
        float zMax = 8f;
        if (CurrentColorType == ColorType.Black && _isThrow)
        {
            Vector3 position = transform.position;

            if (position.x > xMax)
            {
                position.x = xMin;
            }
            else if (position.x < xMin)
            {
                position.x = xMax;
            }

            if (position.y > yMax)
            {
                position.y = yMin;
            }
            else if (position.y < yMin)
            {
                position.y = yMax;
            }

            if (position.z > zMax)
            {
                position.z = zMin;
            }
            else if (position.z < zMin)
            {
                position.z = zMax;
            }

            transform.position = position;
        }
    }
    private IEnumerator HitCoolTime()
    {
        _isHitCoolTime = true;

        yield return new WaitForSeconds(1.0f);
        _isHitCoolTime = false;
    }
    private IEnumerator ScaleChangeCoolTime()
    {
        yield return new WaitForSeconds(1.2f);
        _currentScaleType = ScaleType.Second;
    }
    private IEnumerator AutoUseGrabity()
    {
        yield return new WaitForSeconds(11.0f);
        _rb.useGravity = true;
    }
    private IEnumerator BlackMakuraPlayerSerch()
    {
        yield return new WaitForSeconds(2.0f);
        _targetPosition = TargetPosition();
    }
}
