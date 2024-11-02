using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakuraController : MonoBehaviour
{
    [SerializeField] private LayerMask _groundLayer;
    private Rigidbody _rb;
    private MeshCollider _col;
    private ScaleType _currentType;
    private bool _isThrow = false;
    private bool _hitCoolTime = false;
    private Renderer _renderer;
    private ColorType _currentColor;

    public bool IsThrow { get => _isThrow; set => _isThrow = value; }
    public MeshCollider Col { get => _col; set => _col = value; }
    public GameObject Thrower { get; set; }
    public ScaleType CurrentType { get => _currentType; set => _currentType = value; }
    public ColorType CurrentColor { get => _currentColor; set => _currentColor = value; }

    public enum ScaleType
    {
        Nomal,
        First,
        Second
    }
    public enum ColorType
    {
        Nomal,
        Red,
        Blue,
        Green,
    }
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _col = GetComponent<MeshCollider>();
        _renderer = GetComponent<Renderer>();
        _currentType = ScaleType.Nomal;
        _currentColor = ColorType.Nomal;
    }
    void Update()
    {
        ScaleChange(_currentType);
        ColorChange(_currentColor);
        if (Input.GetKeyDown(KeyCode.B))
        {
            _currentType = ScaleType.First;
            //_currentColor= ColorType.Blue;
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            _currentType = ScaleType.Nomal;
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
    private void ColorChange(ColorType type)
    {
        if (type == ColorType.Nomal)
        {
            _renderer.material.color = Color.white;
        }
        else if (type == ColorType.Red)
        {
            _renderer.material.color = Color.red;
        }
        else if (type == ColorType.Green)
        {
            _renderer.material.color = Color.green;
        }
        else if (type == ColorType.Blue)
        {
            _renderer.material.color = Color.blue;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if ((_groundLayer & (1 << collision.gameObject.layer)) == _groundLayer)
        {
            _isThrow = false;
            _rb.velocity = Vector3.zero;
            _currentType = ScaleType.Nomal;
            _rb.isKinematic = true;
        }
        if (collision.gameObject.CompareTag("Player") && _isThrow && !_hitCoolTime)
        {
            _isThrow = false;
            _rb.velocity = Vector3.zero;
            _currentType = ScaleType.Nomal;
            Debug.Log("敵に当たったぜ");
            StartCoroutine(HitCoolTime());
        }
        if (collision.gameObject.CompareTag("Makura") && _isThrow)
        {
            // if (_currentType == ScaleType.Nomal)
            // {
            //     //TODO
            // }
            _rb.velocity = Vector3.zero;
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
        _currentType = ScaleType.Second;
    }
}
