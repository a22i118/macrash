using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionRange : MonoBehaviour
{
    private bool _isHitCoolTime = false;//当たった時のクールタイム
    private GameObject _thrower;//投げたプレイヤー

    public GameObject Thrower { get => _thrower; set => _thrower = value; }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider collider)
    {
        if (!_isHitCoolTime && collider.gameObject != _thrower)
        {
            if (collider.gameObject.CompareTag("Player") && collider is CapsuleCollider)
            {
                Debug.Log("爆発がヒットしたぜ！");

                StartCoroutine(HitCoolTime());
            }
            if (collider.gameObject.CompareTag("Makura"))
            {
                Rigidbody rb = collider.gameObject.GetComponent<Rigidbody>();
                if (rb != null && !rb.isKinematic)
                {
                    rb.useGravity = true;
                    rb.velocity = Vector3.zero;
                }
            }
        }
    }
    private IEnumerator HitCoolTime()
    {
        _isHitCoolTime = true;

        yield return new WaitForSeconds(1.0f);
        _isHitCoolTime = false;
    }
}
