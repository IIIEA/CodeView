using Sirenix.OdinInspector;
using UnityEngine;

namespace _Fly_Connect.Scripts.Gameplay.Upgrades
{
    public class UpgradeModel : SerializedMonoBehaviour
    {
        [field: SerializeField] public UpgradeType UpgradeType { get; private set; }

        public int Level { get; private set; } = 1;

        public float NextAddedValue { get; private set; } = 10;
        public BigNumber NextPrice { get; private set; } = new(100);
        public bool AdState { get; private set; }

        public void LevelUp()
        {
            Level++;
        }

        public void SetAdState(bool state)
        {
            AdState = state;
        }

        public void UpdateModel(float nextAddedValue, BigNumber nextPrice)
        {
            NextAddedValue = nextAddedValue;
            NextPrice = nextPrice;
        }

        public void SetLevel(int level)
        {
            Level = level;
        }
    }
}