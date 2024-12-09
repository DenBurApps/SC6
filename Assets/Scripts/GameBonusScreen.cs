using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class GameBonusScreen : MonoBehaviour
{
    [SerializeField] private GameObject _buttonsPlane;
    [SerializeField] private Button[] _buttons;
    [SerializeField] private GameBonusPlane[] _gameBonusPlanes;
    [SerializeField] private AudioSource _bonusSound;

    private ScreenVisabilityHandler _screenVisabilityHandler;

    private void Awake()
    {
        _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
    }

    private void OnEnable()
    {
        foreach (var button in _buttons)
        {
            button.onClick.AddListener(OnButtonClicked);
        }

        foreach (var plane in _gameBonusPlanes)
        {
            plane.CollectClicked += CollectGift;
        }
    }

    private void OnDisable()
    {
        foreach (var button in _buttons)
        {
            button.onClick.RemoveListener(OnButtonClicked);
        }

        foreach (var plane in _gameBonusPlanes)
        {
            plane.CollectClicked -= CollectGift;
        }
    }

    private void Start()
    {
        Disable();
    }

    public void Disable()
    {
        _screenVisabilityHandler.DisableScreen();
    }

    public void Enable()
    {
        _screenVisabilityHandler.EnableScreen();
        _buttonsPlane.SetActive(true);
    }

    private void OnButtonClicked()
    {
        int randomIndex = Random.Range(0, _gameBonusPlanes.Length);
        _gameBonusPlanes[randomIndex].gameObject.SetActive(true);
        _bonusSound.Play();
        _buttonsPlane.SetActive(false);
    }

    private void CollectGift(GameBonusType bonusType)
    {
        switch (bonusType)
        {
            case GameBonusType.Gold:
                PlayerBalanceController.IncreaseBalance(100);
                break;
            case GameBonusType.Multiply:
                PlayerBalanceController.SetMultiplier();
                break;
            case GameBonusType.FreeSpins:
                PlayerBalanceController.AddFreeSpins(10);
                break;
        }
        
        Disable();
    }
    
}


