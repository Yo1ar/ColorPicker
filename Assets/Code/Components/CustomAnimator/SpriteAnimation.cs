using System.Threading.Tasks;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace CustomAnimator
{
	[RequireComponent(typeof(SpriteRenderer))]
	public class SpriteAnimation : MonoBehaviour
	{
		[Range(0.1f, 30), SerializeField] private float _fps;
		[SerializeField] private int _startDelay;

		[SerializeField] private CustomAnimation[] _customAnimations;

		private CustomAnimation _currentAnimation;
		private int _currentFrame;

		private SpriteRenderer _spriteRenderer;
		private Cooldown _cooldown;

		private void Awake()
		{
			_spriteRenderer = GetComponent<SpriteRenderer>();
			_cooldown = new Cooldown(1f / _fps);
		}

		private void Start() =>
			StartAnimation("idle");

		private void Update()
		{
			if (_currentAnimation == null)
				return;
			if (!_cooldown.IsReady)
				return;

			ShowCurrentFrame();

			SetNextFrame();

			_cooldown.Reset();
		}

		public async void StartAnimation(string animName)
		{
			CustomAnimation anim = GetAnimationByName(animName);
			
			if (anim == _currentAnimation)
				return;
			
			await AnimationDelay();

			_currentAnimation = anim;
			_currentFrame = GetStartingFrame();
			_cooldown.Reset();
		}

		private Task AnimationDelay() => 
			Task.Delay(_startDelay == 0f ? Random.Range(100, 300) : _startDelay);

		private CustomAnimation GetAnimationByName(string animName)
		{
			foreach (CustomAnimation customAnim in _customAnimations)
				if (customAnim.name == animName)
					return customAnim;

			UnityEngine.Debug.LogError("Can't find custom anim with name " + "\"{animName}\"");
			return null;
		}

		private int GetStartingFrame() =>
			_currentAnimation.randomFrameStart
				? Random.Range(0, _currentAnimation.frames.Length)
				: 0;

		private void ShowCurrentFrame() =>
			_spriteRenderer.sprite = _currentAnimation.frames[_currentFrame];

		private void SetNextFrame() =>
			_currentFrame = GetNextFrame();

		private int GetNextFrame()
		{
			if (!IsLastFrame())
				return _currentFrame + 1;

			if (_currentAnimation.loop)
				return 0;

			_currentAnimation = null;
			return _currentFrame;
		}

		private bool IsLastFrame() =>
			_currentFrame == _currentAnimation.frames.Length - 1;
	}
}