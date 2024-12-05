using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainScreen : MonoBehaviour
{
    [SerializeField] private TMP_Text _balanceText;
    [SerializeField] private DailyGiftController _dailyGiftController;

    private void OnEnable()
    {
        PlayerBalanceController.BalanceChanged += SetBalanceText;
    }

    private void OnDisable()
    {
        PlayerBalanceController.BalanceChanged -= SetBalanceText;
    }

    private void Start()
    {
        _balanceText.text = PlayerBalanceController.CurrentBalance.ToString();
    }

    private void SetBalanceText(int value)
    {
        _balanceText.text = value.ToString();
    }
}
