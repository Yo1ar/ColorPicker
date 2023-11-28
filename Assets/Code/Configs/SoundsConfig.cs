using UnityEngine;

namespace Configs
{
	[CreateAssetMenu(fileName = "SoundsConfig", menuName = "Configs/Sounds config")]
	public class SoundsConfig: ScriptableObject
	{
		[Header("Music")]
		public AudioClip BackgroundMusic;
		[Header("SFX")]
		public AudioClip ClickClip;
		public AudioClip JumpClip;
		public AudioClip JumpClip2;
		public AudioClip FireClip;
		public AudioClip EraseClip;
		public AudioClip SpeedUpClip;
		public AudioClip GetBonusClip;
		public AudioClip DropClip;
		public AudioClip ExplosionClip;
		public AudioClip HurtClip;
		public AudioClip DoorOpenClip;
		public AudioClip DoorCloseClip;
		public AudioClip TorchBurnClip;
		public AudioClip TrapClip;
		public AudioClip MarkerClip;
	}
}