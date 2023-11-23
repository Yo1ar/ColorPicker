using UnityEngine;
using UnityEngine.Pool;

public class ProjectilePool
{
	private readonly Projectile _projectile;
	private readonly IObjectPool<Projectile> _pool;
	private Vector3 _positionOnGet;
	private Vector2 _launchDirection;
	private Vector3 _projectileRotation;

	public ProjectilePool(Projectile projectile)
	{
		_projectile = projectile;
		_pool = new ObjectPool<Projectile>(OnCreate, OnGet, OnRelease);
	}

	public void LaunchProjectile(Vector3 position, Vector2 direction, Vector3 rotation)
	{
		_positionOnGet = position;
		_launchDirection = direction;
		_projectileRotation = rotation;
		_pool.Get();
	}

	private Projectile OnCreate()
	{
		Projectile proj = Object.Instantiate(_projectile);
		proj.SetPool(_pool);
		return proj;
	}

	private void OnGet(Projectile proj)
	{
		Transform t = proj.transform;
		t.position = _positionOnGet;
		t.eulerAngles = _projectileRotation;
		proj.gameObject.SetActive(true);
		proj.Launch(_launchDirection);
	}

	private void OnRelease(Projectile proj) =>
		proj.gameObject.SetActive(false);
}