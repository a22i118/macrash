using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    protected Renderer _renderer;
    protected ColorType _currentColorType = ColorType.Nomal;

    public ColorType CurrentColorType
    {
        get => _currentColorType;
        set
        {
            _currentColorType = value;
            ColorChange(_currentColorType);
        }
    }
    protected virtual void Awake()
    {
        _renderer = GetComponent<Renderer>();
        ColorChange(_currentColorType);
    }

    protected void ColorChange(ColorType type)
    {
        if (_renderer != null)
        {
            if (type == ColorType.Nomal)
            {
                _renderer.material.color = Color.white;
            }
            else if (type == ColorType.Red)
            {
                _renderer.material.color = Color.red;
            }
            else if (type == ColorType.Green)
            {
                _renderer.material.color = Color.green;
            }
            else if (type == ColorType.Blue)
            {
                _renderer.material.color = Color.blue;
            }
        }
    }
    public enum ColorType
    {
        Nomal,
        Red,
        Blue,
        Green,
    }
}
