using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
namespace Player
{
    public class PlayerStatus : MonoBehaviour
    {
        [SerializeField] private Slider _spBar;
        private Image _fillImage;
        private Color _normalColor;
        private Color _maxColor = Color.red;
        private const int _maxSP = 100000;
        private int _currentSP = 0;
        private bool _IsChargeMax = false;

        public int CurrentSP { get => _currentSP; set => _currentSP = value; }
        public bool IsChargeMax { get => _IsChargeMax; set => _IsChargeMax = value; }

        private void Start()
        {
            _currentSP = 0;
            _spBar.maxValue = _maxSP;
            _spBar.value = _currentSP;

            _fillImage = _spBar.fillRect.GetComponent<Image>();
            _normalColor = _fillImage.color;
        }

        private void Update()
        {
            if (_currentSP < _maxSP)
            {
                _IsChargeMax = false;
                _currentSP += 20;
                _currentSP = Mathf.Min(_currentSP, _maxSP);
                _spBar.value = _currentSP;

                ChangeSPBarColor();
            }
            else
            {
                _IsChargeMax = true;
            }
        }

        public void SpUp()
        {
            if (_currentSP < _maxSP)
            {
                _currentSP += 10000;
                _currentSP = Mathf.Min(_currentSP, _maxSP);
                _spBar.value = _currentSP;

                ChangeSPBarColor();
            }
        }

        private void ChangeSPBarColor()
        {
            if (_fillImage != null)
            {
                _fillImage.color = _currentSP >= _maxSP ? _maxColor : _normalColor;
            }
        }
    }

}
