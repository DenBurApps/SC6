using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class MainScreen : MonoBehaviour
{
    [SerializeField] private TMP_Text _balanceText;
    [SerializeField] private DailyGiftController _dailyGiftController;
    [SerializeField] private BonusScreen _bonusScreen;

    private ScreenVisabilityHandler _screenVisabilityHandler;
    
    public event Action BonusClicked;

    private void Awake()
    {
        _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
    }

    private void OnEnable()
    {
        PlayerBalanceController.BalanceChanged += SetBalanceText;
        _bonusScreen.HomeClicked += _screenVisabilityHandler.EnableScreen;
    }

    private void OnDisable()
    {
        PlayerBalanceController.BalanceChanged -= SetBalanceText;
        _bonusScreen.HomeClicked -= _screenVisabilityHandler.EnableScreen;
    }

    private void Start()
    {
        _balanceText.text = PlayerBalanceController.CurrentBalance.ToString();
    }

    public void OnBonusClicked()
    {
        BonusClicked?.Invoke();
        _screenVisabilityHandler.DisableScreen();
    }

    public void DisableScreen()
    {
        _screenVisabilityHandler.DisableScreen();
    }

    public void EnableScreen()
    {
        _screenVisabilityHandler.EnableScreen();
    }

    public void OpenFruitBlast()
    {
        SceneManager.LoadScene("FruitBlastScene");
    }

    public void OpenDessertParade()
    {
        SceneManager.LoadScene("DessertParadeScene");
    }

    public void OpenSweetJackpot()
    {
        SceneManager.LoadScene("SweetJackpotScene");
    }

    private void SetBalanceText(int value)
    {
        _balanceText.text = value.ToString();
    }
}
