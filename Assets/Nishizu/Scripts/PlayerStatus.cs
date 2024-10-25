using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour
{
    [SerializeField] private Slider spBar;
    [SerializeField] private int maxSP = 100000;
    [SerializeField] private int currentSP = 0;
    private void Start()
    {
        maxSP = 100000;
        currentSP = 0;
        spBar.maxValue = maxSP;
        spBar.value = currentSP;
    }

    private void Update()
    {
        if (currentSP < maxSP)
        {
            currentSP += 20;
            currentSP = Mathf.Min(currentSP, maxSP);
            spBar.value = currentSP;
        }
    }
    public void SpUp()
    {
        if (currentSP < maxSP)
        {
            currentSP += 10000;
            currentSP = Mathf.Min(currentSP, maxSP);
            spBar.value = currentSP;
        }
    }

}
