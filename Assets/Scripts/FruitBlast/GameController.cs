using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace FruitBlast
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private LineHolder _lineHolder;
        [SerializeField] private TMP_Text _playerBalance;
        [SerializeField] private BetInputer _betInputer;
        [SerializeField] private LineInputer _lineInputer;

        [SerializeField]
        private List<FruitHolder> _firstWinLine, _secondWinLine, _thirdWinLine, _fourthWinLine, _fifthWinLine;

        private void OnEnable()
        {
            PlayerBalanceController.BalanceChanged += UpdateBalanceText;
        }

        private void OnDisable()
        {
            PlayerBalanceController.BalanceChanged -= UpdateBalanceText;
        }

        private void Start()
        {
            _playerBalance.text = PlayerBalanceController.CurrentBalance.ToString();
            
        }

        private void UpdateBalanceText(int value)
        {
            _playerBalance.text = value.ToString();
        }
    }
}
