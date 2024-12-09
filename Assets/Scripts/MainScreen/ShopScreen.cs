using System;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class ShopScreen : MonoBehaviour
{
    [SerializeField] private TMP_Text _playerBalance;
    
    private ScreenVisabilityHandler _screenVisabilityHandler;
    
    private void Awake()
    {
        _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
    }

    private void Start()
    {
        DisableScreen();
    }

    private void OnEnable()
    {
        PlayerBalanceController.BalanceChanged += UpdatePlayerBalance;
    }

    private void OnDisable()
    {
        PlayerBalanceController.BalanceChanged -= UpdatePlayerBalance;
    }

    public void EnableScreen()
    {
        UpdatePlayerBalance(PlayerBalanceController.CurrentBalance);
        _screenVisabilityHandler.EnableScreen();
    }

    public void DisableScreen()
    {
        _screenVisabilityHandler.DisableScreen();
    }

    private void UpdatePlayerBalance(int value)
    {
        _playerBalance.text = value.ToString();
    }
}
