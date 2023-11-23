using UnityEngine;
using Utils;

public sealed class ColorGiver : ColorCheckerBase
{
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (!other.IsPlayer())
			return;

		if (IsSameColor(@as: PlayerColorHolder))
			return;

		PlayerColorHolder.SetColor(Color);
	}
}