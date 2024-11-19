using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TensionSpriteManager : MonoBehaviour
{
    private bool _isHeartBeat = false;
    private float _startAlpha;

    // private Renderer _warningRenderer;
    private float[] alphaValues = { 0f, 1f, 0.5f, 1f, 0f };

    private float changeDuration = 0.1f;
    private Image _image;

    // Start is called before the first frame update
    void Start()
    {
        _image = GetComponent<Image>();
        _startAlpha = _image.color.a;
        StartCoroutine(ChangeAlphaOverTime());
    }

    // Update is called once per frame
    void Update()
    {

    }
    private IEnumerator ChangeAlphaOverTime()
    {
        for (int i = 0; i < alphaValues.Length; i++)
        {
            float targetAlpha = alphaValues[i];

            float elapsedTime = 0f;
            _startAlpha = _image.color.a;

            while (elapsedTime < changeDuration)
            {
                float alpha = Mathf.Lerp(_startAlpha, targetAlpha, elapsedTime / changeDuration);
                // Color color = _image.color;
                // color.a = alpha;
                // _image.color = color;
                _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, alpha);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, targetAlpha);

            yield return new WaitForSeconds(changeDuration);
        }

        // StartCoroutine(ChangeAlphaOverTime());
    }
    private void HeartBeat()
    {
        if (_isHeartBeat)
        {

        }
    }
    public void Init()
    {
        _isHeartBeat = true;
    }
}
