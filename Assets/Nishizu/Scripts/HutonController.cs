using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HutonController : MonoBehaviour
{
    private GameObject _makura;

    public GameObject Makura { get => _makura; set => _makura = value; }

    public Vector3 GetCenterPosition()
    {
        Collider collider = GetComponent<Collider>();
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
}
