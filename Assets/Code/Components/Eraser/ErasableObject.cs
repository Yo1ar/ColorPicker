using GameCore.GameServices;
using UnityEngine;
using Utils;
using Utils.Constants;

public class ErasableObject : MonoBehaviour, IErasable
{
	[SerializeField] private GameObject _eraserAbove;
	[SerializeField] private float _eraserAboveOffset;
	private FactoryService _factoryService;

	private void Awake()
	{
		_eraserAbove = Instantiate(_eraserAbove, transform.position.AddY(_eraserAboveOffset), Quaternion.identity);
		_eraserAbove.transform.SetParent(transform);
		Highlight(false);
		_factoryService = Services.FactoryService;
	}

	private void Start() =>
		_factoryService.AddErasable(this);

	public void Erase()
	{
		_factoryService.RemoveErasable(this);
		gameObject.SetActive(false);
	}

	public void Highlight(bool value) =>
		_eraserAbove.SetActive(value);

#if UNITY_EDITOR
	private void OnDrawGizmosSelected() =>
		SceneDebugGizmos.DrawHandlesLabel(transform.position + Vector3.up * 2, "Erasable", Colors.Red);
#endif
}