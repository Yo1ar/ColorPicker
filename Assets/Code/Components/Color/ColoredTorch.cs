using GameCore.GameServices;
using UnityEngine;
using UnityEngine.Events;

public sealed class ColoredTorch : ColorCheckerBase
{
	[Header("Torch")] 
	[SerializeField] private GameObject _fire;
	[SerializeField] private UnityEvent _onFireUp;
	
	private AudioSourcesController _audioSourcesController;
	private AudioClip _torchBurnSound;

	protected override void Awake()
	{
		base.Awake();
		_audioSourcesController = Services.AudioService.AudioSourcesController;
		_torchBurnSound = Services.AssetService.SoundsConfig.TorchBurnClip;
	}

	private void Start() =>
		_fire.SetActive(false);

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (!other.TryGetComponent(out ColorHolderBase colorHolder))
			return;

		if (IsSameColor(colorHolder))
		{
			_audioSourcesController.PlaySoundOneShot(_torchBurnSound);
			_fire.SetActive(true);
			_onFireUp.Invoke();
		}
	}
}