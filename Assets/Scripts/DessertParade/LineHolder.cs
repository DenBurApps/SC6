using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace DessertParade
{
    public class LineHolder : MonoBehaviour
    {
        [SerializeField] private Color _notSelectedHorizontalColor;
        [SerializeField] private Color _selectedHorizontalColor;

        [SerializeField] private Sprite _selectedVerticalSprite;
        [SerializeField] private Sprite _defaultVerticalSprite;

        [SerializeField] private Image[] _lines;

        public int LinesCount => _lines.Length;
        public event Action AllLinesActive;
        public event Action AllLinesDisabled;

        private void Start()
        {
            DisableAllLines();
        }

        public void EnableLine()
        {
            for (int i = 0; i < _lines.Length; i++)
            {
                if (!_lines[i].isActiveAndEnabled)
                {
                    _lines[i].gameObject.SetActive(true);

                    if (GetCurrentActiveLinesCount() == _lines.Length)
                        AllLinesActive?.Invoke();

                    return;
                }
            }
        }

        public void HighligthLine(int index)
        {
            if (index <= 5 && index < _lines.Length)
            {
                _lines[index].color = _selectedHorizontalColor;
            }
            else if (index >= 6 && index < _lines.Length && index < 8)
            {
                _lines[index].sprite = _selectedVerticalSprite;
            }
            else if (index >= 8 && index < _lines.Length)
            {
                _lines[index].color = _selectedHorizontalColor;
            }
        }

        public void ReturnAllLinesToDefault()
        {
            for (int i = 0; i < _lines.Length; i++)
            {
                if (i <= 5 && i < _lines.Length)
                {
                    _lines[i].color = _selectedHorizontalColor;
                }
                else if (i >= 6 && i < _lines.Length && i < 8)
                {
                    _lines[i].sprite = _selectedVerticalSprite;
                }
                else if (i >= 8 && i < _lines.Length)
                {
                    _lines[i].color = _selectedHorizontalColor;
                }
            }
        }

        public void DisableLine()
        {
            for (int i = _lines.Length - 1; i > 0; i--)
            {
                if (_lines[i].isActiveAndEnabled)
                {
                    _lines[i].gameObject.SetActive(false);

                    if (GetCurrentActiveLinesCount() == 1)
                        AllLinesDisabled?.Invoke();

                    return;
                }
            }
        }

        public void DisableAllLines()
        {
            for (int i = 1; i < _lines.Length; i++)
            {
                _lines[i].gameObject.SetActive(false);
            }

            AllLinesDisabled?.Invoke();
        }

        public int GetCurrentActiveLinesCount()
        {
            return _lines.Count(line => line.isActiveAndEnabled);
        }
    }
}