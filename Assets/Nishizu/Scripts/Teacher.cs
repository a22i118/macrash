using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teacher : MonoBehaviour
{
    [SerializeField] private GameObject _objMakura;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Angry(Transform targetTransform)
    {
        GameObject makura = Instantiate(_objMakura, transform.position, Quaternion.identity);
        makura.GetComponent<TeaherMakuraController>().Target = targetTransform;
        makura.GetComponent<TeaherMakuraController>().TargetPlayer = targetTransform.gameObject;
    }
}
