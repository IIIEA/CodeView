using InspectorMathExpressions;

public class AirportUpgrade : MathExpression<AirportUpgrade>
{
	[MathExpressionEvaluator]
	public float Get(int level, float exponent, float multiplier) {
		 return (float)EvaluateMathExpression(level, exponent, multiplier);
	}
#if UNITY_EDITOR
	public static partial class ScriptableObjectCreators
	{
		[UnityEditor.MenuItem("Assets/Create/Math expression/AirportUpgrade")]
		public static void CreateAirportUpgrade() { ScriptableObjectUtility.CreateAsset<AirportUpgrade>(); }
	}
#endif
}