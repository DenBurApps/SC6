using System;
using System.Collections;
using System.Collections.Generic;
using TheraBytes.BetterUi;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace FruitBlast
{
    public class SlotReel : MonoBehaviour
    {
        [SerializeField] private List<ItemHolder> _fruitHolders;
        [SerializeField] private float _spinSpeed = 500f;
        [SerializeField] private float _spinDuration = 2f;
        [SerializeField] private List<Item> _fruits;
        [SerializeField] private BetterAxisAlignedLayoutGroup _group;

        private List<Item> _finalResult;
        public event Action StartSpinning;
        public event Action StoppedSpinning;

        private void Start()
        {
            DisableAllStars();
        }

        public void SpinReel()
        {
            _finalResult = GenerateRandomResults();

            StartCoroutine(SpinCoroutine());
        }

        public void DisableAllStars()
        {
            foreach (var fruitHolder in _fruitHolders)
            {
                fruitHolder.ToggleStar(false);
            }
        }

        private IEnumerator SpinCoroutine()
        {
            float elapsedTime = 0f;
            StartSpinning?.Invoke();

            while (elapsedTime < _spinDuration)
            {
                foreach (var fruitHolder in _fruitHolders)
                {
                    fruitHolder.transform.Translate(Vector3.down * (_spinSpeed * Time.deltaTime));

                    if (fruitHolder.transform.localPosition.y <= -fruitHolder.GetHeight() * 3f)
                    {
                        LoopFruitToTop(fruitHolder);
                    }
                }

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            AlignToFinalResult();
        }

        private void LoopFruitToTop(ItemHolder itemHolder)
        {
            float topPosition = GetTopFruitPosition();

            itemHolder.transform.localPosition = new Vector3(
                itemHolder.transform.localPosition.x,
                topPosition + itemHolder.GetHeight(),
                itemHolder.transform.localPosition.z
            );

            itemHolder.SetFruit(_fruits[Random.Range(0, _fruits.Count)]);
        }

        private float GetTopFruitPosition()
        {
            float topY = float.MinValue;
            foreach (var fruitHolder in _fruitHolders)
            {
                if (fruitHolder.transform.localPosition.y > topY)
                    topY = fruitHolder.transform.localPosition.y;
            }

            return topY;
        }

        private void AlignToFinalResult()
        {
            for (int i = 0; i < _fruitHolders.Count; i++)
            {
                _fruitHolders[i].SetFruit(_finalResult[i % _finalResult.Count]);

                float spacing = _fruitHolders[i].GetHeight();
                _fruitHolders[i].transform.localPosition = new Vector3(
                    0,
                    (_fruitHolders.Count - i - 1) * spacing,
                    0
                );
            }

            _group.SetLayoutVertical();
            StoppedSpinning?.Invoke();
        }

        private List<Item> GenerateRandomResults()
        {
            List<Item> results = new List<Item>();
            for (int i = 0; i < _fruitHolders.Count; i++)
            {
                results.Add(_fruits[Random.Range(0, _fruits.Count)]);
            }

            return results;
        }
    }
}