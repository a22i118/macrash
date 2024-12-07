using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Teacher : MonoBehaviour
{
    [SerializeField] private GameObject _objMakura;
    [SerializeField] private GameObject _teacherGuide;
    private bool _isGameStart = false;
    private TextMeshProUGUI _teacherComent;
    public bool IsGameStart { get => _isGameStart; set => _isGameStart = value; }

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0.0f, 1.0f, 6.0f);
        _teacherComent = _teacherGuide.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
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
        StartCoroutine(AngryText());
        GameObject makura = Instantiate(_objMakura, transform.position, Quaternion.identity);
        makura.GetComponent<TeaherMakuraController>().Target = targetTransform;
        makura.GetComponent<TeaherMakuraController>().TargetPlayer = targetTransform.gameObject;
    }
    private IEnumerator AngryText()
    {
        _teacherGuide.SetActive(true);
        _teacherComent.text = "何で起きているんだ!!";
        yield return new WaitForSeconds(3.0f);
        _teacherGuide.SetActive(false);
    }
}
