using Enemy;
using Interface;
using UI;
using UnityEngine;
using UnityEngine.UI; 

namespace Player
{
    public class PlayerUI : MonoBehaviour, IDamageable
    {
        [SerializeField] private GameOverUI gameOverUI;
        [SerializeField] private EnemySpawner spawner;
        [SerializeField] private PlayerCollector collector;
        
        public int maxHealth = 100;
        private int _currentHealth;

        // Optional UI
        public Slider healthBar; // Assign in inspector (optional)
        public Text healthText;  // Assign in inspector (optional)

        private void Start()
        {
            _currentHealth = maxHealth;
            UpdateHealthUI();
        }

        public void TakeDamage(int amount)
        {
            _currentHealth -= amount;
            _currentHealth = Mathf.Max(_currentHealth, 0);
            Debug.Log("Player took damage. Current health: " + _currentHealth);
            UpdateHealthUI();

            if (_currentHealth <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            Debug.Log("Player has died!");
            gameOverUI.Show(spawner.waveNumber - 1, collector.orbCount);
            gameObject.SetActive(false);
            // TODO: Add death animation, disable controls, respawn, etc.
            // For example:
            // GetComponent<PlayerController>().enabled = false;
            // Destroy(gameObject);
        }

        private void UpdateHealthUI()
        {
            if (healthBar != null)
                healthBar.value = (float)_currentHealth / maxHealth;

            if (healthText != null)
                healthText.text = _currentHealth + " / " + maxHealth;
        }

        public void Heal(int amount)
        {
            print("HEALLING" + amount);
            _currentHealth += amount;
            _currentHealth = Mathf.Min(_currentHealth, maxHealth);
            UpdateHealthUI();
        }
    }
}
