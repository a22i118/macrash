using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ButtonController : MonoBehaviour
{
    [SerializeField] private GameObject _firstmenu;
    [SerializeField] private GameObject _config;
    [SerializeField] private GameObject _tutorial;
    [SerializeField] private GameObject _basicsetting;
    [SerializeField] private GameObject _keysetting;
    [SerializeField] private GameObject _exitmenu;
    [SerializeField] private TextMeshProUGUI _text;
    private List<GameObject> _ruleList = new List<GameObject>();
    private int _ruleNum = 0;
    private int _ruleListCount;
    private void Start()
    {
        for (int i = 2; i <= 7; i++)
        {
            _ruleList.Add(_tutorial.transform.GetChild(i).gameObject);
        }
        _ruleListCount = _ruleList.Count;
        RuleMenuSwhich();
    }

    public void ConfigChange(int i)
    {
        if (i == 0)
        {
            _basicsetting.SetActive(true);
            _keysetting.SetActive(false);
        }
        else if (i == 1)
        {
            _basicsetting.SetActive(false);
            _keysetting.SetActive(true);
        }
        else if (i == 2)
        {
            _config.SetActive(false);
            _firstmenu.SetActive(true);
        }
        else if (i == 3)
        {
            _tutorial.SetActive(false);
            _firstmenu.SetActive(true);
        }
        else if (i == 4)
        {
            if (_ruleNum == 0)
            {
                _ruleNum = _ruleListCount-1;
            }
            else
            {
                _ruleNum--;
            }
            RuleMenuSwhich();
        }
        else if (i == 5)
        {
            if (_ruleNum == _ruleListCount-1)
            {
                _ruleNum = 0;
            }
            else
            {
                _ruleNum++;
            }
            RuleMenuSwhich();
        }
    }

    private void RuleMenuSwhich()
    {
        for (int i = 0; i < _ruleListCount; i++)
        {
            if (i == _ruleNum) {
                _ruleList[i].SetActive(true);
            }
            else
            {
                _ruleList[i].SetActive(false);
            }
        }
        _text.text = _ruleNum+1 + " / 6";
    }

    public void StartLocal()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void ExitGame(int i)
    {
        if (i == 0)
        {
            Application.Quit(); //ゲーム終了

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; //エディタ終了
#endif
        }
        else
        {
            _exitmenu.SetActive(false);
            _firstmenu.SetActive(true);
        }
    }

}

