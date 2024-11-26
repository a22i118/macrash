using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HutonController : MonoBehaviour
{
    private GameObject _makura;

    public GameObject Makura { get => _makura; set => _makura = value; }
   

    public Vector3 GetCenterPosition()
    {
        return GetComponent<Collider>().bounds.center;
    }
    public Quaternion GetRotation()
    {
        return transform.rotation;
    }

    // Start is called before the first frame update
    void Start()
    {
        _makura = transform.GetChild(0).gameObject;
        _makura.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Makura") || collision.gameObject.CompareTag("Obstacles"))
        {

            // 布団に触れたプレイヤー以外のオブジェクトを子に設定
            collision.transform.SetParent(transform.parent);
        }
    }
}
