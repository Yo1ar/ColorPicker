using Components.Characters.Player;
using Components.Player;
using UnityEngine;
using UnityEngine.Events;
using Utils;

namespace Components.Level
{
    public class DamagePlayerTrigger : MonoBehaviour
    {
        [SerializeField] private UnityEvent<Transform> _onDamage;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.IsPlayer())
                return;
        
            var playerHealth = other.GetComponent<PlayerHealth>();
            playerHealth.Damage();
        
            _onDamage?.Invoke(other.transform);
        }
    }
}
