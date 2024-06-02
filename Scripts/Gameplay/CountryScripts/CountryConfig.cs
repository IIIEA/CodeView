using InspectorMathExpressions;
using UnityEngine;

namespace _Fly_Connect.Scripts.Gameplay.CountryScripts
{
    public class CountryConfig : MathExpression<CountryConfig>
    {
        [field: SerializeField] public float PriceMultiplier { get; private set; }
        [field: SerializeField] public int StartPrice { get; private set; }

        [MathExpressionEvaluator]
        public int Get(float multiplier, int startPrice, double currentOpenedCountry)
        {
            return (int) EvaluateMathExpression(multiplier, startPrice, currentOpenedCountry);
        }
#if UNITY_EDITOR
        public static partial class ScriptableObjectCreators
        {
            [UnityEditor.MenuItem("Assets/Create/Math expression/CountryConfig")]
            public static void CreateSampleFormula()
            {
                ScriptableObjectUtility.CreateAsset<CountryConfig>();
            }
        }
#endif
    }
}