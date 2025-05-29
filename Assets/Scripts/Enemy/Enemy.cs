using Interface;
using UnityEngine;

namespace Enemy
{
    public class Enemy: MonoBehaviour, IDamageable
    {
        public int health = 3;
        private int _currentHealth;
        public System.Action OnDeath;
        
        [Header("Drop Settings")]
        public GameObject orbPrefab;
        public Transform dropPoint; 

        private void Start()
        {
            _currentHealth = health;
        }

        public void TakeDamage(int damage)
        {
            _currentHealth -= damage;

            if (_currentHealth <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            if (orbPrefab)
            {
                print("DROPU ORBE");
                Instantiate(orbPrefab, dropPoint.position, Quaternion.identity);
            }
            OnDeath?.Invoke();
            Destroy(gameObject);
            
        }
    }
}
