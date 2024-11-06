using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MakuraMeteor : MonoBehaviour
{
    private Rigidbody _rb;
    private MeshCollider _col;
    public MeshCollider Col { get => _col; set => _col = value; }
    private Action _onDisable;
    private float _accumulate; //�o�ߎ���
    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        Col = GetComponent<MeshCollider>();
        _rb.constraints = RigidbodyConstraints.FreezeRotation; 
    }

    void Update()
    { 
        //�����Ȃ������Ƃ��Ɏ��Ԍo�߂Ńv�[���ɕԂ�
        _accumulate += Time.deltaTime;

        if (_accumulate >= 5)   
        {
            _onDisable?.Invoke();
            gameObject.SetActive(false);

        }    
        
    }
    //�I�u�W�F�N�g�̏�����
    public void Initialize(Action onDisable)
    {
        //��A�N�e�B�u���̍ۂɎ��s�����R�[���o�b�N�̐ݒ�
        _onDisable = onDisable;
        _accumulate = 0;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //���̂ɓ���������v�[���ɕԂ�
        if (gameObject.activeSelf)
        {
            _onDisable?.Invoke();
            gameObject.SetActive(false);
        }
        
    }
    private void OnDisable()
    {
        _rb.velocity = Vector3.zero;
    }
}