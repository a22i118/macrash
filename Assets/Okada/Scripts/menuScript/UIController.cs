using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{

    Animator anim;
    MeshRenderer mesh;

    void Start()
    {
        anim = GetComponent<Animator>();
        //mesh = GetComponent<MeshRenderer>();
        //StartCoroutine("Blink");
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void OnMouseEnter()
    {
        // �}�E�X���I�u�W�F�N�g��ɏ�����Ƃ�
        //Debug.Log($"{gameObject.name} �Ƀ}�E�X�����܂���");
        anim.SetBool("_isBig", true);
    }

    void OnMouseExit()
    {
        // �}�E�X���I�u�W�F�N�g���痣�ꂽ�Ƃ�
        //Debug.Log($"{gameObject.name} ����}�E�X������܂���");
        anim.SetBool("_isBig", false);
    }

    //IEnumerator Blink()
    //{
    //    while (true)
    //    {
    //        for (int i = 0; i < 100; i++)
    //        {
    //            mesh.material.color = mesh.material.color - new Color32(0, 0, 0, 1);
    //        }

    //        yield return new WaitForSeconds(0.2f);

    //        for (int k = 0; k < 100; k++)
    //        {
    //            mesh.material.color = mesh.material.color + new Color32(0, 0, 0, 1);
    //        }

    //        yield return new WaitForSeconds(0.2f);

    //    }
    //}
}
