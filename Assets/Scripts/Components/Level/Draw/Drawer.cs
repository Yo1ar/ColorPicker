using System.Threading.Tasks;
using GameCore.GameServices;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

namespace Components.Level.Draw
{
	public sealed class Drawer : MonoBehaviour
	{
		private Drawable[] _drawables;
		private DrawingHand _drawingHand;
		private IObjectPool<DrawingHand> _objectPool;

		private void Awake()
		{
			_drawingHand = ServiceLocator.assetService.DrawingHand;
			
			CreateObjectPool();
			GetAllDrawables();
		}

		private void CreateObjectPool()
		{
			_objectPool =
				new ObjectPool<DrawingHand>(
					createFunc: OnCreate,
					actionOnGet: OnGet,
					actionOnRelease: OnRelease);
		}

		private void OnEnable()
		{
			foreach (Drawable drawable in _drawables)
			{
				drawable.OnVisible += Draw;
				drawable.SetInvisible();
			}
		}

		private void OnDisable()
		{
			foreach (Drawable drawable in _drawables)
				drawable.OnVisible -= Draw;
		}

		private async void Draw(Vector3 position, Drawable drawable)
		{
			await RandomDelay();

			_objectPool.Get(out DrawingHand hand);
			hand.transform.position = position;
			drawable.SetVisible();
		}

		private DrawingHand OnCreate()
		{
			DrawingHand hand = InstantiateHand();
			hand.SetPool(_objectPool);
			return hand;
		}

		private DrawingHand InstantiateHand() => 
			Instantiate(_drawingHand, Vector3.zero, Quaternion.identity, transform);

		private void OnGet(DrawingHand hand) =>
			hand.gameObject.SetActive(true);

		private void OnRelease(DrawingHand hand) =>
			hand.gameObject.SetActive(false);

		private void GetAllDrawables() =>
			_drawables = FindObjectsOfType<Drawable>();

		private static Task RandomDelay() =>
			Task.Delay((int) (Random.Range(0.1f, 0.5f) * 1000));
	}
}