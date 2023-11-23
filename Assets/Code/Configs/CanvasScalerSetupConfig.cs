using UnityEngine;
using UnityEngine.UI;

namespace Configs
{
	[CreateAssetMenu(fileName = "CanvasScalerSetupConfig", menuName = "Configs/CanvasScaler config")]
	public class CanvasScalerSetupConfig : ScriptableObject
	{
		public Vector2 ReferenceResolution = new Vector2(800, 600);
		public CanvasScaler.ScreenMatchMode ScreenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
		[Range(0, 1)]public float Match = 0;
	}
}