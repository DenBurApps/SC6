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
        [SerializeField] private List<FruitHolder> _fruitHolders;
        [SerializeField] private float _spinSpeed = 500f;
        [SerializeField] private float _spinDuration = 2f;
        [SerializeField] private List<Fruit> _fruits;
        [SerializeField] private BetterAxisAlignedLayoutGroup _group;
        
        private List<Fruit> _finalResult;
        public event Action StartSpinning;
        public event Action StoppedSpinning;

        public void SpinReel()
        {
            _finalResult = GenerateRandomResults();

            StartCoroutine(SpinCoroutine());
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

        private void LoopFruitToTop(FruitHolder fruitHolder)
        {
            float topPosition = GetTopFruitPosition();
            
            fruitHolder.transform.localPosition = new Vector3(
                fruitHolder.transform.localPosition.x,
                topPosition + fruitHolder.GetHeight(),
                fruitHolder.transform.localPosition.z
            );
            
            fruitHolder.SetFruit(_fruits[Random.Range(0, _fruits.Count)]);
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

        private List<Fruit> GenerateRandomResults()
        {
            List<Fruit> results = new List<Fruit>();
            for (int i = 0; i < _fruitHolders.Count; i++)
            {
                results.Add(_fruits[Random.Range(0, _fruits.Count)]);
            }

            return results;
        }
    }
}