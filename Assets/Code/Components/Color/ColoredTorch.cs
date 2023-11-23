using UnityEngine;
using UnityEngine.Events;
using Utils.Constants;

public sealed class ColoredTorch : ColorCheckerBase
{
	[Header("Torch")] [SerializeField] private GameObject _fire;
	[SerializeField] private UnityEvent _onFireUp;

	private void Start()
	{
		_fire.SetActive(false);
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag(Tags.ENEMY))
			return;

		if (!other.TryGetComponent(out ColorHolderBase colorHolder))
			return;

		if (IsSameColor(colorHolder))
		{
			_fire.SetActive(true);
			_onFireUp.Invoke();
		}
	}
}