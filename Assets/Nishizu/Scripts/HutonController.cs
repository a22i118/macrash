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

            // �z�c�ɐG�ꂽ�v���C���[�ȊO�̃I�u�W�F�N�g���q�ɐݒ�
            collision.transform.SetParent(transform.parent);
        }
    }
}
