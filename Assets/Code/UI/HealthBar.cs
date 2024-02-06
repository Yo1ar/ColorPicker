namespace UI
{
	public sealed class HealthBar : UiElement
	{
		private HealthBarController _healthBarController;

		protected override void Awake()
		{
			base.Awake();
			_healthBarController = new HealthBarController(Document);
		}

		private void OnDisable() =>
			_healthBarController.Dispose();
	}
}