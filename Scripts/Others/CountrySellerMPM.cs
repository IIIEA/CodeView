using InspectorMathExpressions;

public class CountrySellerMPM : MathExpression<CountrySellerMPM>
{
	[MathExpressionEvaluator]
	public float Get(float mpm, float citiesInChosenCountry, float m) {
		 return (float)EvaluateMathExpression(mpm, citiesInChosenCountry, m);
	}
#if UNITY_EDITOR
	public static partial class ScriptableObjectCreators
	{
		[UnityEditor.MenuItem("Assets/Create/Math expression/CountrySellerMPM")]
		public static void CreateCountrySellerMPM() { ScriptableObjectUtility.CreateAsset<CountrySellerMPM>(); }
	}
#endif
}