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
