using System;
using UnityEngine;
using UnityEngine.UI;

namespace FruitBlast
{
    [RequireComponent(typeof(Image))]
    public class FruitHolder : MonoBehaviour
    {
        [SerializeField] private Image _fruitImage;
        
        private Image _starImage;
        
        public Fruit Fruit { get; private set; }

        private void Awake()
        {
            _starImage = GetComponent<Image>();
        }

        private void OnEnable()
        {
            _starImage.enabled = false;
        }

        public void SetFruit(Fruit data)
        {
            Fruit = data;
            _fruitImage.sprite = Fruit.Sprite;
        }

        public void EnableStar()
        {
            _starImage.enabled = true;
        }
        
        public float GetHeight()
        {
            return _starImage.rectTransform.rect.height;
        }
    }
}
