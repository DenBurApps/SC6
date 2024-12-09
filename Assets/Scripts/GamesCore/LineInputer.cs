using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore
{
    public class LineInputer : MonoBehaviour
    {
        [SerializeField] private Button _plusButton;
        [SerializeField] private Button _minusButton;
        [SerializeField] private TMP_Text _currentLineText;
        [SerializeField] private LineHolder _lineHolder;
        [SerializeField] private DessertParade.LineHolder _dplineHolder;

        private int _minLines = 1;

        private void OnEnable()
        {
            _plusButton.onClick.AddListener(IncreaseLine);
            _minusButton.onClick.AddListener(DecreaseLine);

            if (_lineHolder != null)
                _lineHolder.AllLinesDisabled += UpdateLineDisplay;

            if (_dplineHolder != null)
                _dplineHolder.AllLinesDisabled += UpdateLineDisplay;
        }

        private void OnDisable()
        {
            _plusButton.onClick.RemoveListener(IncreaseLine);
            _minusButton.onClick.RemoveListener(DecreaseLine);

            if (_lineHolder != null)
                _lineHolder.AllLinesDisabled -= UpdateLineDisplay;

            if (_dplineHolder != null)
                _dplineHolder.AllLinesDisabled -= UpdateLineDisplay;
        }

        private void IncreaseLine()
        {
            if (_lineHolder != null)
                _lineHolder.EnableLine();

            if (_dplineHolder != null)
                _dplineHolder.EnableLine();

            UpdateLineDisplay();
            ToggleButtons();
        }

        private void DecreaseLine()
        {
            if (_lineHolder != null)
                _lineHolder.DisableLine();

            if (_dplineHolder != null)
                _dplineHolder.DisableLine();

            UpdateLineDisplay();
            ToggleButtons();
        }

        private void UpdateLineDisplay()
        {
            if (_lineHolder != null)
                _currentLineText.text = _lineHolder.GetCurrentActiveLinesCount().ToString();

            if (_dplineHolder != null)
                _currentLineText.text = _dplineHolder.GetCurrentActiveLinesCount().ToString();

            ToggleButtons();
        }

        private void ToggleButtons()
        {
            int activeLines = 0;

            if (_lineHolder != null)
            {
                activeLines = _lineHolder.GetCurrentActiveLinesCount();
                _plusButton.interactable = activeLines < _lineHolder.LinesCount;
            }

            if (_dplineHolder != null)
            {
                activeLines = _dplineHolder.GetCurrentActiveLinesCount();
                _plusButton.interactable = activeLines < _dplineHolder.LinesCount;
            }
            
            _minusButton.interactable = activeLines > _minLines;
        }
    }
}