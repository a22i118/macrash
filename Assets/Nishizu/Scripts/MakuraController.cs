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
    private float _vibrationStrength = 0.05f;//振動の強さ
    private float _vibrationTime = 0.2f;//振動する時間
    [SerializeField] private GameObject _explosionRange;
    private ExplosionRange _explosionRangeScript;
    private bool _isCharge = false;
    private bool _isCounterAttack = false;
    public bool IsThrow { get => _isThrow; set => _isThrow = value; }
    public GameObject Thrower { get => _thrower; set => _thrower = value; }
    public ScaleType CurrentScaleType { get => _currentScaleType; set => _currentScaleType = value; }
    public bool IsAlterEgo { get => _isAlterEgo; set => _isAlterEgo = value; }
    public bool IsCharge { get => _isCharge; set => _isCharge = value; }
    public bool IsHitCoolTime { get => _isHitCoolTime; set => _isHitCoolTime = value; }
    public bool IsCounterAttack { get => _isCounterAttack; set => _isCounterAttack = value; }

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
            if (_rb.useGravity && !_isTouching)
            {
                _col.isTrigger = false;
            }
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
                // _isCounterAttack = false;
            }

            if (collision.gameObject.CompareTag("Player") && _isThrow && !_isHitCoolTime && collision.gameObject != _thrower)
            {

                _currentScaleType = ScaleType.Nomal;
                _rb.useGravity = true;
                _rb.isKinematic = true;
                _rb.isKinematic = false;
                _rb.velocity = Vector3.zero;

                StartCoroutine(HitStopVibration());
                StartCoroutine(HitCoolTime());
                Debug.Log("敵に当たったぜ");
            }
            if (collision.gameObject.CompareTag("Makura"))
            {
                _isThrow = false;
                _currentScaleType = ScaleType.Nomal;
                _rb.useGravity = true;
                _rb.velocity = Vector3.zero;
                transform.rotation = Quaternion.Euler(_initialRotation.eulerAngles.x, transform.rotation.eulerAngles.y, _initialRotation.eulerAngles.z);
            }
        }

        if (((_groundLayer & (1 << collision.gameObject.layer)) == _groundLayer) || collision.gameObject.CompareTag("Huton"))
        {
            _isThrow = false;
            _isCounterAttack = false;
            _currentScaleType = ScaleType.Nomal;
            transform.rotation = Quaternion.Euler(_initialRotation.eulerAngles.x, transform.rotation.eulerAngles.y, _initialRotation.eulerAngles.z);
            if (!_rb.isKinematic)
            {
                _rb.velocity = Vector3.zero;
            }
            _rb.isKinematic = true;
            _currentColorType = GetRandomColor();
        }
        if (_isCharge)
        {
            HitSpawn();
            _isCharge = false;
        }
        if (_isAlterEgo)
        {
            Destroy(gameObject, 0.3f);
        }


    }
    private void HitSpawn()
    {
        GameObject spawnedObject = Instantiate(_explosionRange, transform.position, Quaternion.identity);
        if (_explosionRange != null)
        {
            _explosionRangeScript = spawnedObject.GetComponent<ExplosionRange>();
        }
        _explosionRangeScript.Thrower = _thrower;
        Destroy(spawnedObject, 2.0f);
    }
    private void OnTriggerEnter(Collider collider)
    {
        _isTouching = true;
        if (_col.isTrigger && _isThrow && !_isHitCoolTime && collider.gameObject != _thrower)
        {
            if (collider.gameObject.CompareTag("Player") && collider is CapsuleCollider)
            {
                StartCoroutine(BlackMakuraHit());
                StartCoroutine(HitCoolTime());
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
    private ColorType GetRandomColor()
    {
        const int nomal = 30;
        const int blue = 20;
        const int green = 20;
        const int red = 15;
        const int black = 15;
        int all = nomal + red + blue + green + black;
        float randomValue = UnityEngine.Random.Range(0, all);

        if (randomValue < nomal)
        {
            return ColorType.Nomal;
        }
        else if (randomValue < nomal + blue)
        {
            return ColorType.Blue;
        }
        else if (randomValue < nomal + blue + green)
        {
            return ColorType.Green;
        }
        else if (randomValue < nomal + blue + green + red)
        {
            return ColorType.Red;
        }
        else
        {
            return ColorType.Black;
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
    public IEnumerator HitStopVibration()
    {
        Vector3 hitPosition = transform.position;

        float elapsedTime = 0.0f;
        while (elapsedTime < _vibrationTime)
        {
            Vector3 randomOffset = new Vector3(
                UnityEngine.Random.Range(-_vibrationStrength, _vibrationStrength),
                0,
                UnityEngine.Random.Range(-_vibrationStrength, _vibrationStrength)
            );

            transform.position = hitPosition + randomOffset;

            elapsedTime += 0.05f;
            yield return new WaitForSeconds(0.05f);
        }

        transform.position = hitPosition;

        _isHitCoolTime = false;
        _isThrow = false;
    }
    private IEnumerator BlackMakuraHit()
    {
        yield return new WaitForSeconds(0.01f);
        _col.isTrigger = false;

        _currentScaleType = ScaleType.Nomal;
        _rb.useGravity = true;
        _rb.isKinematic = true;
        _rb.isKinematic = false;
        StartCoroutine(HitStopVibration());
    }
}
