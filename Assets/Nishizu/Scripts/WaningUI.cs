using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningUI : MonoBehaviour
{
    [SerializeField] GameObject _warningPrefab;
    [SerializeField] private float _generationTimeInterval = 1.285f;
    private bool _isWarning = false;
    private bool _isNearingEnd = false;
    private bool _isExecuteOnce = false; //一回だけ実行する
    private float _timer = 0.0f;
    private float _generationInterval;
    public float _num = 1000f;

    // Start is called before the first frame update
    void Start()
    {
        _generationInterval = _warningPrefab.GetComponent<RectTransform>().sizeDelta.x * _warningPrefab.GetComponent<RectTransform>().localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isWarning)
        {
            _timer += Time.deltaTime;

            if (_timer >= _generationTimeInterval && !_isExecuteOnce)
            {
                if (!_isNearingEnd)
                {
                    GeneratesWarning(_isNearingEnd);
                }
                else
                {
                    _isExecuteOnce = true;
                    GeneratesWarning(_isNearingEnd);
                }

                _timer = 0.0f;
            }
        }
    }

    private void GeneratesWarning(bool isNearingEnd)
    {
        if (!isNearingEnd)
        {
            // 上部のWarning UIを生成
            GameObject warningTop = Instantiate(_warningPrefab, transform);
            RectTransform rectTransformTop = warningTop.GetComponent<RectTransform>();
            rectTransformTop.anchorMin = new Vector2(0.5f, 1f); // 上部中央にアンカーを設定
            rectTransformTop.anchorMax = new Vector2(0.5f, 1f);
            rectTransformTop.pivot = new Vector2(0.5f, 0.5f); // ピボットを中央に
            rectTransformTop.anchoredPosition = new Vector2(0f, -510f);

            // 下部のWarning UIを生成
            GameObject warningUnder = Instantiate(_warningPrefab, transform);
            RectTransform rectTransformUnder = warningUnder.GetComponent<RectTransform>();
            rectTransformUnder.anchorMin = new Vector2(0.5f, 0f); // 下部中央にアンカーを設定
            rectTransformUnder.anchorMax = new Vector2(0.5f, 0f);
            rectTransformUnder.pivot = new Vector2(0.5f, 0.5f); // ピボットを中央に
            rectTransformUnder.anchoredPosition = new Vector2(0f, 510f);
            warningUnder.GetComponent<WarningMove>().Direction = false;
        }
        else
        {
            for (int i = 0; i < 4; i++)
            {
                // 上部のWarning UIを生成
                GameObject warningTop = Instantiate(_warningPrefab, transform);
                RectTransform rectTransformTop = warningTop.GetComponent<RectTransform>();
                rectTransformTop.anchorMin = new Vector2(0.5f, 1f);
                rectTransformTop.anchorMax = new Vector2(0.5f, 1f);
                rectTransformTop.pivot = new Vector2(0.5f, 0.5f);
                rectTransformTop.anchoredPosition = new Vector2(_generationInterval * i + _num, -510f); // スライドさせるためX軸を調整

                // 下部のWarning UIを生成
                GameObject warningUnder = Instantiate(_warningPrefab, transform);
                RectTransform rectTransformUnder = warningUnder.GetComponent<RectTransform>();
                rectTransformUnder.anchorMin = new Vector2(0.5f, 0f);
                rectTransformUnder.anchorMax = new Vector2(0.5f, 0f);
                rectTransformUnder.pivot = new Vector2(0.5f, 0.5f);
                rectTransformUnder.anchoredPosition = new Vector2(-_generationInterval * i - _num, 510f); // スライドさせるためX軸を調整

                warningTop.GetComponent<WarningMove>().StartCoroutine(warningTop.GetComponent<WarningMove>().FadeOutCoroutine());
                warningUnder.GetComponent<WarningMove>().StartCoroutine(warningUnder.GetComponent<WarningMove>().FadeOutCoroutine());
                warningUnder.GetComponent<WarningMove>().Direction = false;
            }
        }
    }

    public void Init()
    {
        _isWarning = true;
        _isNearingEnd = false;
        _isExecuteOnce = false;
        StartCoroutine(WarningCoroutine());
        for (int i = 0; i < 4; i++)
        {
            // 上部のWarning UIを生成
            GameObject warningTop = Instantiate(_warningPrefab, transform);
            RectTransform rectTransformTop = warningTop.GetComponent<RectTransform>();
            rectTransformTop.anchorMin = new Vector2(0.5f, 1f);
            rectTransformTop.anchorMax = new Vector2(0.5f, 1f);
            rectTransformTop.pivot = new Vector2(0.5f, 0.5f);
            rectTransformTop.anchoredPosition = new Vector2(-_generationInterval * i + _num, -510f); // 初期位置を調整

            // 下部のWarning UIを生成
            GameObject warningUnder = Instantiate(_warningPrefab, transform);
            RectTransform rectTransformUnder = warningUnder.GetComponent<RectTransform>();
            rectTransformUnder.anchorMin = new Vector2(0.5f, 0f);
            rectTransformUnder.anchorMax = new Vector2(0.5f, 0f);
            rectTransformUnder.pivot = new Vector2(0.5f, 0.5f);
            rectTransformUnder.anchoredPosition = new Vector2(_generationInterval * i - _num, 510f); // 初期位置を調整

            warningTop.GetComponent<WarningMove>().IsTransparent = true;
            warningUnder.GetComponent<WarningMove>().IsTransparent = true;
            warningTop.GetComponent<WarningMove>().StartCoroutine(warningTop.GetComponent<WarningMove>().FadeInCoroutine());
            warningUnder.GetComponent<WarningMove>().StartCoroutine(warningUnder.GetComponent<WarningMove>().FadeInCoroutine());

            warningUnder.GetComponent<WarningMove>().Direction = false;
        }
    }

    private IEnumerator WarningCoroutine()
    {
        yield return new WaitForSeconds(1.0f);
        _isNearingEnd = true;
    }
}