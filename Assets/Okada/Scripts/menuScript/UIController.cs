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
        // マウスがオブジェクト上に乗ったとき
        //Debug.Log($"{gameObject.name} にマウスが乗りました");
        anim.SetBool("_isBig", true);
    }

    void OnMouseExit()
    {
        // マウスがオブジェクトから離れたとき
        //Debug.Log($"{gameObject.name} からマウスが離れました");
        anim.SetBool("_isBig", false);
    }

    
}
