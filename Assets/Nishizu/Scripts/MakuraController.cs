using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakuraController : MonoBehaviour
{
    [SerializeField] private LayerMask _groundLayer;
    private Rigidbody _rb;
    private MeshCollider _col;
    private bool _isThrow = false;
    private bool _hitCoolTime = false;
    public bool IsThrow { get => _isThrow; set => _isThrow = value; }
    public MeshCollider Col { get => _col; set => _col = value; }
    public GameObject Thrower { get; set; }


    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        Col = GetComponent<MeshCollider>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ((_groundLayer & (1 << collision.gameObject.layer)) == _groundLayer)
        {
            _isThrow = false;
            _rb.velocity = Vector3.zero;
            _rb.isKinematic = true;
        }
        if (collision.gameObject.CompareTag("Player") && _isThrow && !_hitCoolTime)
        {
            _isThrow = false;
            _rb.velocity = Vector3.zero;

            Debug.Log("敵に当たったぜ");
            StartCoroutine(HitCoolTime());
        }
    }
    private IEnumerator HitCoolTime()
    {
        _hitCoolTime = true;

        yield return new WaitForSeconds(1.0f);
        _hitCoolTime = false;
    }
}
