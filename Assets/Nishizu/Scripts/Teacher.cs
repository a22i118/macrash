using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teacher : MonoBehaviour
{
    [SerializeField] private GameObject _objMakura;
    private bool _isGameStart = false;

    public bool IsGameStart { get => _isGameStart; set => _isGameStart = value; }

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0.0f, 1.0f, 6.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (_isGameStart)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(0.0f, 1.0f, 10.0f), Time.deltaTime);
        }
    }
    public void Angry(Transform targetTransform)
    {
        GameObject makura = Instantiate(_objMakura, transform.position, Quaternion.identity);
        makura.GetComponent<TeaherMakuraController>().Target = targetTransform;
        makura.GetComponent<TeaherMakuraController>().TargetPlayer = targetTransform.gameObject;
    }
}
