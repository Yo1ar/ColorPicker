using UnityEngine;

[SelectionBase]
public sealed class ColoredSpikes : ColorCheckerBase
{
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (!other.TryGetComponent(out IHealth health))
			return;

		health.Kill();
	}
}