using System;
using System.Collections.Generic;
using System.Linq;
using GameCore;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GameCore
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private DessertParade.LineHolder _dplineHolder;
        [SerializeField] private LineHolder _lineHolder;
        [SerializeField] private TMP_Text _playerBalance;
        [SerializeField] private BetInputer _betInputer;
        [SerializeField] private LineInputer _lineInputer;
        [SerializeField] private Button _spinButton;
        [SerializeField] private Button _homeButton;
        [SerializeField] private GameBonusScreen _gameBonusScreen;
        [SerializeField] private GameObject _notEnoughPopup;
        [SerializeField] private List<WinLine> _winLines;
        [SerializeField] private AudioSource _spinSound;
        [SerializeField] private AudioSource _winSound;

        [SerializeField] private List<SlotReel> _reels;

        private int _currentBet;
        private int _currentLines;
        private int _reelsStopped;
        private bool _playedOnce;

        private void OnEnable()
        {
            PlayerBalanceController.BalanceChanged += UpdateBalanceText;
            _spinButton.onClick.AddListener(OnSpinClicked);
            _homeButton.onClick.AddListener(ReturnToMainScene);

            foreach (var reel in _reels)
            {
                reel.StartSpinning += () => _spinButton.interactable = false;
                reel.StoppedSpinning += OnReelStopped;
            }
        }

        private void OnDisable()
        {
            PlayerBalanceController.BalanceChanged -= UpdateBalanceText;
            _spinButton.onClick.RemoveListener(OnSpinClicked);
            _homeButton.onClick.RemoveListener(ReturnToMainScene);

            foreach (var reel in _reels)
            {
                reel.StartSpinning -= () => _spinButton.interactable = false;
                reel.StoppedSpinning -= OnReelStopped;
            }
        }

        private void Start()
        {
            _notEnoughPopup.SetActive(false);
            _playedOnce = false;
            _playerBalance.text = PlayerBalanceController.CurrentBalance.ToString();
        }

        private void OnSpinClicked()
        {
            _currentBet = _betInputer.CurrentBet;

            if (_lineHolder != null)
                _currentLines = _lineHolder.GetCurrentActiveLinesCount();

            if (_dplineHolder != null)
                _currentLines = _dplineHolder.GetCurrentActiveLinesCount();

            int totalBetCost = _currentBet * _currentLines;

            if (PlayerBalanceController.CurrentBalance < totalBetCost)
            {
                _notEnoughPopup.SetActive(true);
                return;
            }

            StartSpin();
            _spinSound.Play();
            _playedOnce = true;
        }

        private void OnReelStopped()
        {
            _reelsStopped++;
            if (_reelsStopped >= _reels.Count)
            {
                VerifyResults();
                _reelsStopped = 0;
            }
        }

        private void UpdateBalanceText(int value)
        {
            _playerBalance.text = value.ToString();
        }

        private void StartSpin()
        {
            if (_lineHolder != null)
                _lineHolder.ReturnAllLinesToDefault();

            if (_dplineHolder != null)
                _dplineHolder.ReturnAllLinesToDefault();

            DisableAllStars();
            foreach (var reel in _reels)
            {
                reel.SpinReel();
            }

            PlayerBalanceController.DecreaseBalance(_currentBet * _currentLines);
        }

        private void VerifyResults()
        {
            var linesToCheck = _winLines.Take(_currentLines).ToList();
            int winAmount = 0;

            for (int i = 0; i < linesToCheck.Count; i++)
            {
                var lineWin = CalculateLineWin(linesToCheck[i].ItemHolders, i);
                winAmount += lineWin;
            }

            if (winAmount > 0)
            {
                _winSound.Play();
                PlayerBalanceController.IncreaseBalance(winAmount);
            }

            _spinButton.interactable = true;
            Debug.Log(winAmount);
        }

        private int CalculateLineWin(List<ItemHolder> fruitHolders, int lineIndex)
        {
            int win = 0;

            var groupedFruits = fruitHolders.GroupBy(fruitHolder => fruitHolder.Item.Type);

            foreach (var group in groupedFruits)
            {
                if (group.Count() >= 3)
                {
                    if (group.Key == Type.Bonus)
                    {
                        _gameBonusScreen.Enable();
                    }

                    foreach (var fruitHolder in group)
                    {
                        win += fruitHolder.Item.Value;
                        fruitHolder.ToggleStar(true);
                    }

                    if (_lineHolder != null)
                        _lineHolder.HighligthLine(lineIndex);

                    if (_dplineHolder != null)
                        _dplineHolder.HighligthLine(lineIndex);
                }
            }

            return win;
        }

        private void ReturnToMainScene()
        {
            if (_playedOnce)
                BonusProgressSaver.IncreaseFruitBlastProgress();

            SceneManager.LoadScene("MainScene");
        }

        private void DisableAllStars()
        {
            foreach (var reel in _reels)
            {
                reel.DisableAllStars();
            }
        }
    }
}

[Serializable]
public class WinLine
{
    public List<ItemHolder> ItemHolders;
}