using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUIManager : MonoBehaviour
{
    private enum UItype
    {
        VS,
        Practice,
        Config,
        Exit,
        Local,
        Internet

    }

    Animator anim;
    UItype _currenttype;
    [SerializeField] private GameObject _makura;
    [SerializeField] private GameObject VSmenu;
    [SerializeField] private GameObject Firstmenu;
    [SerializeField] private GameObject LocalVSmenu;
    Rigidbody _rb;
    [SerializeField] float _throwforce;
    Vector3 _throwposition;
    private bool _isray = true;

    private void Start()
    {
        anim = GetComponent<Animator>();
        _throwposition = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y - 1, Camera.main.transform.position.z);
    }

    private void Update()
    {
        if (_isray)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 30, ~5))
            {

                CurrentUI(hit.collider.gameObject.name);

                if (Input.GetMouseButtonDown(0))
                {
                    ThrowMakura(hit.collider.gameObject.transform.position);
                    StartCoroutine(MoveMenu());
                    _isray = false;
                }
            }
            
        }
        

        
    }
    // Start is called before the first frame update
    //void OnMouseEnter()
    //{
    //    // マウスがオブジェクト上に乗ったとき
    //    //Debug.Log($"{gameObject.name} にマウスが乗りました");
    //    anim.SetBool("_isBig", true);
    //}

    //void OnMouseExit()
    //{
    //    // マウスがオブジェクトから離れたとき
    //    //Debug.Log($"{gameObject.name} からマウスが離れました");
    //    anim.SetBool("_isBig", false);
    //}
    

    private void CurrentUI(string UIname)
    {
        switch (UIname)
        {
            case "VSSymbol":
                _currenttype = UItype.VS;
                break;

            case "PracticeSymbol":
                _currenttype= UItype.Practice;
                break;

            case "ConfigSymbol":
                _currenttype= UItype.Config;
                break;

            case "ExitSymbol":
                _currenttype = UItype.Exit;
                break;

            case "Local":
                _currenttype = UItype.Local;
                break;

            case "Internet":
                _currenttype = UItype.Internet;
                break;
        }
    }

    

    private void ThrowMakura(Vector3 position)
    {
        Vector3 _throwdirection = position - _throwposition;
        _throwdirection.Normalize();

        GameObject _throwmakura = Instantiate(_makura, _throwposition, Quaternion.identity);
        _rb = _throwmakura.GetComponent<Rigidbody>();
        
        if (_rb != null)
        {
            _rb.AddForce(_throwdirection * _throwforce, ForceMode.VelocityChange);
            
        }
    }

    private IEnumerator MoveMenu()
    {
        yield return null;
        switch (_currenttype)
        {
            case UItype.VS:
                VSmenu.SetActive(true);
                Firstmenu.SetActive(false);
                break;

            case UItype.Practice:
                break;

            case UItype.Config:
                break;

           

            
        }
    }

    private void OnUI()
    {
        switch (_currenttype)
        {
            case UItype.VS:
                
                break;

            case UItype.Practice:
                break;

            case UItype.Config:
                break;




        }
    }
    public void LocalVSMenu()
    {
        LocalVSmenu.SetActive(true);
        VSmenu.SetActive(false);
    }
}
