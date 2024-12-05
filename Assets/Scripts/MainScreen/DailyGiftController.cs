using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class DailyGiftController : MonoBehaviour
{
    [SerializeField] private Image _giftImage;
    [SerializeField] private Button _giftButton;
    [SerializeField] private Sprite _collectedSprite;
    [SerializeField] private Sprite _notCollectedSprite;
    [SerializeField] private List<DailyGift> _dailyGifts;
    [SerializeField] private ScreenVisabilityHandler _screenVisabilityHandler;
    
    private string _savePath;

    private DateTime _lastCollectedDate;
    
    public event Action SpinsCollected;
    public event Action MultiplyCollected;

    public bool GiftCollected { get; private set; }

    private void Awake()
    {
        _savePath = Path.Combine(Application.persistentDataPath, "GiftSave");
    }

    private void Start()
    {
        Load();
    }

    private void OnEnable()
    {
        foreach (var gift in _dailyGifts)
        {
            gift.CollectClicked += OnGiftCollected;
        }

        _giftButton.onClick.AddListener(GetGift);
    }

    private void OnDisable()
    {
        foreach (var gift in _dailyGifts)
        {
            gift.CollectClicked -= OnGiftCollected;
        }

        _giftButton.onClick.RemoveListener(GetGift);
    }

    private void SaveGiftData()
    {
        DailyGiftInfoSaver wrapper = new DailyGiftInfoSaver(DateTime.Today);
        string json = JsonConvert.SerializeObject(wrapper, Formatting.Indented);

        if (File.Exists(_savePath) && File.ReadAllText(_savePath) == json) return;

        File.WriteAllText(_savePath, json);
        Debug.Log($"Data saved to: {_savePath}");
    }

    private void Load()
    {
        try
        {
            if (!File.Exists(_savePath))
            {
                Debug.Log("No save file found.");
                ResetGift();
                return;
            }

            var json = File.ReadAllText(_savePath);
            var wrapper = JsonConvert.DeserializeObject<DailyGiftInfoSaver>(json);

            if (DateTime.Today > wrapper.CollectedGiftDate)
            {
                ResetGift();
                return;
            }

            _giftButton.enabled = false;
            _giftImage.sprite = _collectedSprite;
            GiftCollected = true;
            _screenVisabilityHandler.DisableScreen();
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error loading gift data: {ex.Message}");
            ResetGift();
        }
    }

    private void ResetGift()
    {
        _giftButton.enabled = true;
        _giftImage.sprite = _notCollectedSprite;
        GiftCollected = false;
        _screenVisabilityHandler.DisableScreen();
    }

    private void GetGift()
    {
        if (GiftCollected)
            return;
        
        _screenVisabilityHandler.EnableScreen();
        var randomGift = Random.Range(0, _dailyGifts.Count);
        _dailyGifts[randomGift].gameObject.SetActive(true);
    }

    private void OnGiftCollected(GiftType type)
    {
        switch (type)
        {
            case GiftType.Gold:
                PlayerBalanceController.IncreaseBalance(100);
                break;
            case GiftType.Multiply:
                MultiplyCollected?.Invoke();
                break;
            case GiftType.GoldSpins:
                PlayerBalanceController.IncreaseBalance(100);
                SpinsCollected?.Invoke();
                break;
        }

        _giftButton.enabled = false;
        _giftImage.sprite = _collectedSprite;
        GiftCollected = true;
        _screenVisabilityHandler.DisableScreen();
        SaveGiftData();
    }
}

[Serializable]
public class DailyGiftInfoSaver
{
    public DateTime CollectedGiftDate;

    public DailyGiftInfoSaver(DateTime collectedGiftDate)
    {
        CollectedGiftDate = collectedGiftDate;
    }
}