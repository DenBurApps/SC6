using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameBonusPlane : MonoBehaviour
{
    [SerializeField] private GameBonusType _type;
    [SerializeField] private Button _collectButton;

    public event Action<GameBonusType> CollectClicked;

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
        CollectClicked?.Invoke(_type);
        gameObject.SetActive(false);
    }

}

public enum GameBonusType
{
    Gold,
    Multiply,
    FreeSpins
}
