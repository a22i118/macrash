using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowMakuraController : MonoBehaviour
{
   [SerializeField]
    private ScaleType _currentType;
    public ScaleType CurrentType { get => _currentType; set => _currentType = value; }
    public enum ScaleType
    {
        Nomal,
        First,
        Second
    }
    // Start is called before the first frame update
    void Start()
    {
        _currentType = ScaleType.Nomal;
    }

    // Update is called once per frame
    void Update()
    {
        ScaleChange(_currentType);
    }
    private void ScaleChange(ScaleType type)
    {
        if (type == ScaleType.Second)
        {
            transform.localScale = new Vector3(6.0f, 6.0f, 6.0f);
        }
        else if (type == ScaleType.First)
        {
            transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
            StartCoroutine(ScaleChangeCoolTime());
        }
        else if (type == ScaleType.Nomal)
        {
            transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        }
    }
    private IEnumerator ScaleChangeCoolTime()
    {
        yield return new WaitForSeconds(1.2f);
        _currentType = ScaleType.Second;
    }
}
