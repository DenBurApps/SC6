using UnityEngine;

namespace FruitBlast
{
    [CreateAssetMenu(fileName = "New item", menuName = "Items")]
    public class Item : ScriptableObject
    {
        [SerializeField] private Sprite _sprite;
        [SerializeField] private int _value;
        [SerializeField] private Type _type;

        public Sprite Sprite => _sprite;
        public int Value => _value;
        public Type Type => _type;
    }

    public enum Type
    {
        Bonus,
        [Header("FruitBlast")]
        Apple,
        Lemon,
        Pineapple,
        Berry,
        Orange,
        Melon,
        Grape,
        Strawberry,
        [Header("SweetJackpot")]
        Cheesecake,
        Candy,
        Lollipop,
        Donut,
        Biscuits,
        Cake,
        [Header("DessertParade")]
        BigCake,
        Pie,
        Marshmallow,
        Pudding,
        Macaron,
        IceCream
    }
}