using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class BonusScreen : MonoBehaviour
{
    [SerializeField] private Color _notCompleteColor;
    [SerializeField] private Color _completeColor;

    [SerializeField] private TMP_Text[] _progressTexts;
    [SerializeField] private Image[] _progressImages;
    [SerializeField] private ScreenVisabilityHandler _bonusHolder;
    [SerializeField] private List<Bonus> _bonuses;

    private ScreenVisabilityHandler _screenVisabilityHandler;
    private string _savePath;

    private void Awake()
    {
        _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
        
    }

    private void OnEnable()
    {
        foreach (var bonus in _bonuses)
        {
        }
    }

    private void Start()
    {
        _screenVisabilityHandler.DisableScreen();
    }

    private void EnableScreen()
    {
        _screenVisabilityHandler.EnableScreen();
        _bonusHolder.DisableScreen();
    }

    private void ActivateBonus()
    {
    }

    

    private void ResetBonusesInfo()
    {
        foreach (var progress in _progressTexts)
        {
            progress.text = "0/5";
        }
        
        foreach (var image in _progressImages)
        {
            image.color = _notCompleteColor;
        }
    }

    private void SetProgress()
    {
        
    }
}
