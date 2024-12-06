using System;
using UnityEngine;
using UnityEngine.UI;

namespace FruitBlast
{
    [RequireComponent(typeof(Image))]
    public class ItemHolder : MonoBehaviour
    {
        [SerializeField] private Image _fruitImage;
        
        private Image _starImage;
        
        public Item Item { get; private set; }

        private void Awake()
        {
            _starImage = GetComponent<Image>();
        }

        private void OnEnable()
        {
            ToggleStar(false);
        }

        public void SetFruit(Item data)
        {
            Item = data;
            _fruitImage.sprite = Item.Sprite;
        }

        public void ToggleStar(bool status)
        {
            _starImage.enabled = status;
        }
        
        public float GetHeight()
        {
            return _starImage.rectTransform.rect.height;
        }
    }
}
