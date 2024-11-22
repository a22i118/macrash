using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockController : MonoBehaviour
{
    [SerializeField] private GameObject _hourHand;
    [SerializeField] private GameObject _minuteHand;

    private float _hour = 10.0f;//時
    private float _minute = 0.0f;//分
    private float _oneLap = 0.333333f;//3分で一周

    public float Hour { get => _hour; }
    public float Minute { get => _minute; }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        _minute += Time.deltaTime;
        if (_minute >= 60.0f)
        {
            _minute = 0.0f;
            _hour++;
            if (_hour >= 12.0f)
            {
                _hour = 0.0f;
            }
        }
        HandRotation();
    }

    private void HandRotation()
    {
        float minuteRotation = _minute * _oneLap * 6.0f;
        _minuteHand.transform.localRotation = Quaternion.Euler(0.0f, -minuteRotation, 0.0f);

        float hourRotation = (_hour % 12.0f + _minute / 60.0f) * 30.0f;
        _hourHand.transform.localRotation = Quaternion.Euler(0.0f, -hourRotation, 0.0f);
    }
}
