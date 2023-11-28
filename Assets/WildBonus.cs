using DG.Tweening;
using GameCore.GameServices;
using UnityEngine;
using Utils;

public sealed class WildBonus : MonoBehaviour
{
	[SerializeField] private float _rotationTime;
	private readonly Vector3 _endValue = new(0, 0, 360);
	private Sequence _sequence;
	private AudioSourcesController _audioSourcesController;
	private AudioClip _bonusSound;

	private void Awake()
	{
		_audioSourcesController = Services.AudioService.AudioSourcesController;
		_bonusSound = Services.AssetService.SoundsConfig.GetBonusClip;
	}

	private void OnEnable() =>
		transform
			.DORotate(_endValue, _rotationTime, RotateMode.FastBeyond360)
			.SetLoops(-1)
			.SetEase(Ease.Linear)
			.Play();

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (!other.IsPlayer())
			return;

		if (!other.TryGetComponent(out IWildColorContainer wildColorContainer))
			return;
		
		_audioSourcesController.PlaySoundOneShot(_bonusSound);
		wildColorContainer.AddWildColorBonus();
		gameObject.SetActive(false);
	}
}