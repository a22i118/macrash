using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultHutonController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Transform firstChild = transform.GetChild(0);

        Renderer childRenderer = firstChild.GetComponent<Renderer>();

        if (childRenderer != null)
        {
            childRenderer.material.color = new Color(1.0f, 0.843f, 0.0f);
            //Color(0.75f, 0.75f, 0.75f)//銀
            //Color(0.72f, 0.45f, 0.2f)//銅
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
