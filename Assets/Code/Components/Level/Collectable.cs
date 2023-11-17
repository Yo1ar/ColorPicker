using Components.Characters.Player;
using UnityEngine;
using Utils;

namespace Components.Level
{
	public class Collectable : MonoBehaviour
	{
		[SerializeField] private EPlayerSkills _item;

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (!other.IsPlayer())
				return;

			var inv = other.GetComponent<PlayerSkills>();
			inv.AddSkillStack(_item);
			gameObject.SetActive(false);
		}
	}
}