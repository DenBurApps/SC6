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
    [SerializeField] private Button[] _openBonusButtons;
    [SerializeField] private Image[] _progressImages;
    [SerializeField] private ScreenVisabilityHandler _bonusHolder;
    [SerializeField] private List<Bonus> _bonuses;
    [SerializeField] private MainScreen _mainScreen;
    [SerializeField] private AudioSource _bonusSound;

    private ScreenVisabilityHandler _screenVisabilityHandler;
    private string _savePath;
    private List<BonusData> _bonusDatas = new List<BonusData>(4);
    
    public event Action HomeClicked;

    private void Awake()
    {
        _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
        
        for (int i = 0; i < 4; i++)
        {
            _bonusDatas.Add(new BonusData());
        }
    }

    private void OnEnable()
    {
        for (int i = 0; i < _openBonusButtons.Length; i++)
        {
            int index = i;
            _openBonusButtons[i].onClick.AddListener(() => ActivateBonus(index));
        }

        foreach (var bonuse in _bonuses)
        {
            bonuse.CollectClicked += ActivateBonusType;
        }

        _mainScreen.BonusClicked += EnableScreen;
    }

    private void OnDisable()
    {
        for (int i = 0; i < _openBonusButtons.Length; i++)
        {
            int index = i;
            _openBonusButtons[i].onClick.RemoveListener(() => ActivateBonus(index));
        }

        foreach (var bonuse in _bonuses)
        {
            bonuse.CollectClicked -= ActivateBonusType;
        }
    }

    private void Start()
    {
        _screenVisabilityHandler.DisableScreen();
        _bonusHolder.DisableScreen();
    }

    public void OnHomeClicked()
    {
        HomeClicked?.Invoke();
        _screenVisabilityHandler.DisableScreen();
    }

    private void LoadBonuses()
    {
        _bonusDatas[0].Progress = BonusProgressSaver.SweetJackpot5Progress;
        _bonusDatas[0].IsCollected = BonusProgressSaver.IsSweetJackpot5Collected;
        _bonusDatas[0].TargetProgress = 5;

        _bonusDatas[1].Progress = BonusProgressSaver.FruitBlast10Progress;
        _bonusDatas[1].IsCollected = BonusProgressSaver.IsFruitBlast10Collected;
        _bonusDatas[1].TargetProgress = 10;

        _bonusDatas[2].Progress = BonusProgressSaver.DessertParade5Progress;
        _bonusDatas[2].IsCollected = BonusProgressSaver.IsDessertParade5Collected;
        _bonusDatas[2].TargetProgress = 5;

        _bonusDatas[3].Progress = BonusProgressSaver.SweetJackpot10Progress;
        _bonusDatas[3].IsCollected = BonusProgressSaver.IsSweetJackpot10Collected;
        _bonusDatas[3].TargetProgress = 10;
    }


    private void EnableScreen()
    {
        _screenVisabilityHandler.EnableScreen();
        _bonusHolder.DisableScreen();
        LoadBonuses();
        SetProgress();
    }

    public void ActivateBonus(int bonusIndex)
    {
        if (bonusIndex < 0 || bonusIndex >= _bonusDatas.Count) return;

        var bonus = _bonusDatas[bonusIndex];
        if (bonus.Progress < bonus.TargetProgress || bonus.IsCollected) return;

        bonus.IsCollected = true;

        _bonusHolder.EnableScreen();

        switch (bonusIndex)
        {
            case 0:
                BonusProgressSaver.CollectedSweetJackpot5();
                _bonuses[0].gameObject.SetActive(true);
                break;
            case 1:
                BonusProgressSaver.CollectedFruitBlast10();
                _bonuses[1].gameObject.SetActive(true);
                break;
            case 2:
                BonusProgressSaver.CollectedDessertParade5();
                _bonuses[0].gameObject.SetActive(true);
                break;
            case 3:
                BonusProgressSaver.CollectedSweetJackpot10();
                _bonuses[1].gameObject.SetActive(true);
                break;
        }

        SetProgress();
    }

    private void SetProgress()
    {
        for (int i = 0; i < _bonusDatas.Count; i++)
        {
            Debug.Log(_progressTexts[i].text = $"{_bonusDatas[i].Progress}/{_bonusDatas[i].TargetProgress}");
            _progressTexts[i].text = $"{_bonusDatas[i].Progress}/{_bonusDatas[i].TargetProgress}";
            _progressImages[i].color = _bonusDatas[i].Progress >= _bonusDatas[i].TargetProgress
                ? _completeColor
                : _notCompleteColor;

            _openBonusButtons[i].enabled = !_bonusDatas[i].IsCollected;
        }
    }

    private void ActivateBonusType(BonusType type)
    {
        _bonusHolder.DisableScreen();
        
        switch (type)
        {
            case BonusType.Gold:
                PlayerBalanceController.IncreaseBalance(100);
                break;
            case BonusType.GoldSpins:
                PlayerBalanceController.IncreaseBalance(100);
                PlayerBalanceController.AddFreeSpins(3);
                break;
        }
    }
}


[Serializable]
public class BonusData
{
    public int Progress;
    public int TargetProgress;
    public bool IsCollected;
}