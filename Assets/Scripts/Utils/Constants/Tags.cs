namespace Utils.Constants
{
	public static class Tags
	{
		public const string Player = "Player";
		public const string Collectable = "Collectable";
		public const string Damageable = "Damageable";
		public const string Healable = "Healable";
		public const string Respawn = "Respawn";
		public const string Finish = "Finish";
		public const string Ground = "Ground";
	}
	
	public enum GameTag
	{
		Player,
		Enemy,
		Respawn,
		Finish,
	}
}