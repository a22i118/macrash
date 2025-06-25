using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowManager : MonoBehaviour
{
    private float _amplitude = 0.35f; // �㉺�ړ��̕�
    private float _frequency = 5f;    // �����̑���

    private Vector3 _startPosition;

    void Start()
    {
        _startPosition = transform.localPosition;
    }

    void Update()
    {
        float newY = Mathf.Sin(Time.time * _frequency ) * _amplitude;
        transform.localPosition = _startPosition + new Vector3(0f, newY, 0f);
    }

}
