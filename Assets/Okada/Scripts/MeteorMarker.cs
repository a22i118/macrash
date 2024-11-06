using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorMarker : MonoBehaviour
{
    private Action _onDisable;
    private float _accumulate; //�o�ߎ���

    void Update()
    {
        //���Ԍo�߂ŏ���
        _accumulate += Time.deltaTime;

        if (_accumulate >= 3)
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

    
}
