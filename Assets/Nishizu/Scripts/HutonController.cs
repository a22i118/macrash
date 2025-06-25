using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HutonController : MonoBehaviour
{
    [SerializeField] private GameObject _arrowUI;
    private GameObject _arrowInstance;
    private GameObject _makura;
    private Vector3 _uiOffset = new Vector3(0f, 1.5f, 0f);
    private bool _canSleep = true;
    public GameObject Makura { get => _makura; set => _makura = value; }
    public bool CanSleep { get => _canSleep; set => _canSleep = value; }

    /// <summary>
    /// 布団のポジションを返す（自分自身）
    /// </summary>
    /// <returns></returns>
    public Vector3 GetCenterPosition()
    {
        return GetComponent<Collider>().bounds.center;
    }
    /// <summary>
    /// 布団の向きを返す（自分自身）
    /// </summary>
    /// <returns></returns>
    public Quaternion GetRotation()
    {
        return transform.rotation;
    }

    // Start is called before the first frame update
    void Start()
    {
        _makura = transform.GetChild(0).gameObject;
        _makura.SetActive(false);

        _arrowInstance = Instantiate(_arrowUI, transform.position + _uiOffset, Quaternion.Euler(15f, 0f, 0f));
        _arrowInstance.transform.SetParent(this.transform);
    }

    // Update is called once per frame
    void Update()
    {
        if (CanSleep)
        {
            _arrowInstance.SetActive(true);
        }
        else
        {
            _arrowInstance.SetActive(false);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Makura") || collision.gameObject.CompareTag("Obstacles"))
        {
            collision.transform.SetParent(transform.parent);
        }
    }
}
