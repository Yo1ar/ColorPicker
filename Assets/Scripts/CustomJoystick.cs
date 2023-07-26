using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.OnScreen;

public class CustomJoystick : OnScreenControl, IPointerUpHandler, IPointerDownHandler, IDragHandler
{
	[InputControl(layout = "Vector2"), SerializeField]
	private string _controlPath;

	[SerializeField] private JoystickDirection _direction;
	[SerializeField] private float _handleMoveRange;

	[Space(), Header("Visual"), SerializeField]
	private RectTransform _background;

	[SerializeField] private RectTransform _handle;

	private RectTransform _transform;
	private Vector2 _defaultBackgroundPosition;
	private Vector2 _defaultHandlePosition;

	private const float HandleSizeOffset = 0.7f;

	protected override string controlPathInternal
	{
		get => _controlPath;
		set => _controlPath = value;
	}

	private void Awake()
	{
		_transform = transform as RectTransform;
	}

	private void Start()
	{
		_defaultBackgroundPosition = _background.anchoredPosition;
		_defaultHandlePosition = _handle.anchoredPosition;
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		_background.anchoredPosition = eventData.pressPosition;
		OnDrag(eventData);
	}

	public void OnDrag(PointerEventData eventData)
	{
		Vector2 delta = eventData.position - eventData.pressPosition;
		Vector2 deltaClamped = Vector2.ClampMagnitude(delta, _handleMoveRange);
		Vector2 deltaWithDirection = ApplyDirection(deltaClamped);

		SendValueToControl(deltaWithDirection);
		
		_handle.anchoredPosition = deltaWithDirection;
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		SendValueToControl(Vector2.zero);

		_background.anchoredPosition = _defaultBackgroundPosition;
		_handle.anchoredPosition = _defaultHandlePosition;
	}

	private Vector2 ApplyDirection(Vector2 deltaClamped)
	{
		switch (_direction)
		{
			case JoystickDirection.Free:
				return deltaClamped;
			case JoystickDirection.Horizontal:
				return new Vector2(deltaClamped.x, _handle.anchoredPosition.y);
			case JoystickDirection.Vertical:
				return new Vector2(_handle.anchoredPosition.x, deltaClamped.y);
			default:
				throw new ArgumentOutOfRangeException();
		}
	}

	private Vector2 ClampInCircle(Vector2 position, Vector2 pressPosition, float radius)
	{
		Vector2 direction = position - pressPosition;

		if (direction.magnitude > radius)
			return pressPosition + direction.normalized * radius;
		else
			return position;
	}

	private enum JoystickDirection
	{
		Free = 0,
		Horizontal = 1,
		Vertical = 2
	}
#if UNITY_EDITOR

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.magenta;

		switch (_direction)
		{
			case JoystickDirection.Free:
				Gizmos.DrawWireSphere(_background.anchoredPosition, _handleMoveRange);
				break;
			case JoystickDirection.Horizontal:
				Gizmos.DrawWireCube(
					_background.anchoredPosition,
					new Vector3(_handleMoveRange * 2, _handle.rect.height * HandleSizeOffset));
				break;
			case JoystickDirection.Vertical:
				Gizmos.DrawWireCube(
					_background.anchoredPosition,
					new Vector3(_handle.rect.width * HandleSizeOffset, _handleMoveRange * 2));
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}
	}

#endif //UNITY_EDITOR
}