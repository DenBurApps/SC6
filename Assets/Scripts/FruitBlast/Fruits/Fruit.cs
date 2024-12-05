using UnityEngine;

namespace FruitBlast
{
    [CreateAssetMenu(fileName = "New fruit", menuName = "Fruit")]
    public class Fruit : ScriptableObject
    {
        [SerializeField] private Sprite _sprite;
        [SerializeField] private int _value;
        [SerializeField] private FruitType _type;

        public Sprite Sprite => _sprite;
        public int Value => _value;
        public FruitType Type => _type;
    }

    public enum FruitType
    {
        Apple,
        Lemon,
        Pineapple,
        Berry,
        Orange,
        Melon,
        Grape,
        Strawberry,
        Caramel
    }
}