using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(SpriteRenderer))]
public sealed class SetRandomSprite : MonoBehaviour
{
	[SerializeField] private Sprite[] _sprites;
	private SpriteRenderer _spriteRenderer;

	private void Awake() =>
		_spriteRenderer = GetComponent<SpriteRenderer>();

	private void Start() =>
		_spriteRenderer.sprite = _sprites[Random.Range(0, _sprites.Length)];
}