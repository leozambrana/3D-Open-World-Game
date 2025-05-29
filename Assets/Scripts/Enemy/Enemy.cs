using Interface;
using UnityEngine;

namespace Enemy
{
    public class Enemy: MonoBehaviour, IDamageable
    {
        public int health = 3;
        private int _currentHealth;

        private void Start()
        {
            _currentHealth = health;
        }

        public void TakeDamage(int damage)
        {
            print("TINHA QUE ENTRAR");
            _currentHealth -= damage;
            Debug.Log("Inimigo levou dano! Vida restante: " + _currentHealth);

            if (_currentHealth <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            Debug.Log("Inimigo morreu!");
            Destroy(gameObject);
        }
    }
}
