using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bonus : MonoBehaviour
{
    [SerializeField] private BonusType _bonusType;
    [SerializeField] private Button _collectButton;
    
    public event Action<BonusType> CollectClicked;

    private void OnEnable()
    {
        _collectButton.onClick.AddListener(OnCollectClicked);
    }

    private void OnDisable()
    {
        _collectButton.onClick.RemoveListener(OnCollectClicked);
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void OnCollectClicked()
    {
        CollectClicked?.Invoke(_bonusType);
        gameObject.SetActive(false);
    }
}

public enum BonusType
{
    Gold,
    GoldSpins,
    None
}
