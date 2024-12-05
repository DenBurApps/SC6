using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FruitBlast
{
    public class BetInputer : MonoBehaviour
    {
        [SerializeField] private Button _plusButton;
        [SerializeField] private Button _minusButton;
        [SerializeField] private TMP_Text _currentBetText;

        private int _currentBet = 10;

        public int CurrentBet => _currentBet;

        private void OnEnable()
        {
            _plusButton.onClick.AddListener(IncreaseBet);
            _minusButton.onClick.AddListener(DecreaseBet);
        }

        private void OnDisable()
        {
            _plusButton.onClick.RemoveListener(IncreaseBet);
            _minusButton.onClick.RemoveListener(DecreaseBet);
        }

        private void Start()
        {
            UpdateBetDisplay();
            ToggleButtons();
        }

        private void IncreaseBet()
        {
            _currentBet = Mathf.Clamp(_currentBet + 10, 10, PlayerBalanceController.CurrentBalance);
            
            UpdateBetDisplay();
            ToggleButtons();
        }

        private void DecreaseBet()
        {
            _currentBet = Mathf.Clamp(_currentBet - 10, 10, PlayerBalanceController.CurrentBalance);
            
            UpdateBetDisplay();
            ToggleButtons();
        }

        private void UpdateBetDisplay()
        {
            _currentBetText.text = _currentBet.ToString();
        }

        private void ToggleButtons()
        {
            _plusButton.interactable = _currentBet + 10 <= PlayerBalanceController.CurrentBalance;
            _minusButton.interactable = _currentBet > 10;
        }
    }
}