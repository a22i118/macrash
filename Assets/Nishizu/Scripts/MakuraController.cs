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
    private bool _hitCoolTime = false;//当たった時のクールタイム
    // private Renderer _renderer;
    // private ColorType _currentColorType = ColorType.Nomal;
    private bool _alterEgo = false;//分身
    private Quaternion _initialRotation;//最初の向き
    private GameObject _thrower;//投げたプレイヤー
    private Vector3 _targetPosition;
    public bool IsThrow { get => _isThrow; set => _isThrow = value; }
    public GameObject Thrower { get => _thrower; set => _thrower = value; }
    public ScaleType CurrentScaleType { get => _currentScaleType; set => _currentScaleType = value; }
    // public ColorType CurrentColorType { get => _currentColorType; set => _currentColorType = value; }
    public bool AlterEgo { get => _alterEgo; set => _alterEgo = value; }
    public enum ScaleType
    {
        Nomal,
        First,
        Second
    }
    // public enum ColorType
    // {
    //     Nomal,
    //     Red,//等速直線
    //     Blue,//分身
    //     Green,//
    // }

    void Start()
    {
        // base.Start();
        _rb = GetComponent<Rigidbody>();
        _col = GetComponent<Collider>();
        // _renderer = GetComponent<Renderer>();
        _initialRotation = transform.rotation;
    }
    void Update()
    {
        ScaleChange(_currentScaleType);
        ColorChange(_currentColorType);
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
        if (CurrentColorType != ColorType.Red && !_rb.useGravity && _rb.velocity != Vector3.zero)
        {
            _rb.useGravity = true;
            _rb.velocity = Vector3.zero;
        }
        if (((_groundLayer & (1 << collision.gameObject.layer)) == _groundLayer) || collision.gameObject.CompareTag("Huton"))
        {
            _isThrow = false;
            _currentScaleType = ScaleType.Nomal;
            transform.rotation = Quaternion.Euler(_initialRotation.eulerAngles.x, transform.rotation.eulerAngles.y, _initialRotation.eulerAngles.z);
            _rb.isKinematic = true;
        }
        if (collision.gameObject.CompareTag("Player") && _isThrow && !_hitCoolTime && collision.gameObject != _thrower)
        {

            _isThrow = false;
            _currentScaleType = ScaleType.Nomal;
            _rb.useGravity = true;
            _rb.velocity = Vector3.zero;
            Debug.Log("敵に当たったぜ");
            StartCoroutine(HitCoolTime());
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
            // if (_currentType == ScaleType.Nomal)
            // {
            //     //TODO
            // }
        }
        if (_alterEgo)
        {
            Destroy(gameObject);
        }
        // if (collision.gameObject.CompareTag("Wall") && _isThrow && _currentColorType == ColorType.Red)
        // {
        //     Vector3 normal = collision.contacts[0].normal;

        //     Vector3 reflectDirection = Vector3.Reflect(_rb.velocity, normal);

        //     _rb.velocity = reflectDirection.normalized * _rb.velocity.magnitude;
        // }
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
        if (_col.isTrigger && _isThrow && !_hitCoolTime && collider.gameObject != _thrower)
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
    private IEnumerator HitCoolTime()
    {
        _hitCoolTime = true;

        yield return new WaitForSeconds(1.0f);
        _hitCoolTime = false;
    }
    private IEnumerator ScaleChangeCoolTime()
    {
        yield return new WaitForSeconds(1.2f);
        _currentScaleType = ScaleType.Second;
    }
    private IEnumerator AutoUseGrabity()
    {
        yield return new WaitForSeconds(10.0f);
        _rb.useGravity = true;
    }
    private IEnumerator BlackMakuraPlayerSerch()
    {
        yield return new WaitForSeconds(2.0f);
        _targetPosition = TargetPosition();
    }
}
