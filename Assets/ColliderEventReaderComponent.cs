using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CapsuleCollider2D))]
public class ColliderEventReaderComponent : MonoBehaviour
{
	[SerializeField] private bool _isTrigger = true;
	[HideInInspector] public UnityEvent<GameObject> OnEnter;
	[HideInInspector] public UnityEvent<GameObject> OnStay;
	[HideInInspector] public UnityEvent<GameObject> OnExit;
	private CapsuleCollider2D _collider;

	private void Awake() =>
		SetupCollider();

	private void OnTriggerEnter2D(Collider2D other) =>
		OnEnter?.Invoke(other.gameObject);

	private void OnTriggerStay2D(Collider2D other) =>
		OnStay?.Invoke(other.gameObject);

	private void OnTriggerExit2D(Collider2D other) =>
		OnExit?.Invoke(other.gameObject);

	private void OnCollisionEnter2D(Collision2D other) =>
		OnEnter?.Invoke(other.gameObject);

	private void OnCollisionStay2D(Collision2D other) =>
		OnStay?.Invoke(other.gameObject);

	private void OnCollisionExit2D(Collision2D other) =>
		OnExit?.Invoke(other.gameObject);
	
	public void RemoveAllListeners()
	{
		OnEnter.RemoveAllListeners();
		OnStay.RemoveAllListeners();
		OnExit.RemoveAllListeners();
	}

	private void SetupCollider()
	{
		_collider = GetComponent<CapsuleCollider2D>();
		_collider.isTrigger = _isTrigger;
	}
}
