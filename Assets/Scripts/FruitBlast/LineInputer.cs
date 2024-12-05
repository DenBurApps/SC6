using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FruitBlast
{
    public class LineInputer : MonoBehaviour
    {
        [SerializeField] private Button _plusButton;
        [SerializeField] private Button _minusButton;
        [SerializeField] private TMP_Text _currentLineText;
        [SerializeField] private LineHolder _lineHolder;

        private int _minLines = 1;

        private void OnEnable()
        {
            _plusButton.onClick.AddListener(IncreaseLine);
            _minusButton.onClick.AddListener(DecreaseLine);
            _lineHolder.AllLinesDisabled += UpdateLineDisplay;
        }

        private void OnDisable()
        {
            _plusButton.onClick.RemoveListener(IncreaseLine);
            _minusButton.onClick.RemoveListener(DecreaseLine);
            _lineHolder.AllLinesDisabled -= UpdateLineDisplay;
        }

        private void IncreaseLine()
        {
            _lineHolder.EnableLine();
            UpdateLineDisplay();
            ToggleButtons();
        }

        private void DecreaseLine()
        {
            _lineHolder.DisableLine();
            UpdateLineDisplay();
            ToggleButtons();
        }

        private void UpdateLineDisplay()
        {
            _currentLineText.text = _lineHolder.GetCurrentActiveLinesCount().ToString();
            ToggleButtons();
        }

        private void ToggleButtons()
        {
            int activeLines = _lineHolder.GetCurrentActiveLinesCount();
            _plusButton.interactable = activeLines < _lineHolder.LinesCount;
            _minusButton.interactable = activeLines > _minLines; 
        }
    }
}