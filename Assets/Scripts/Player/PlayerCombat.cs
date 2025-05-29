using Interface;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerCombat : MonoBehaviour
    {
        [Header("Attack Settings")]
        public Transform attackPoint;
        public float attackRange = 1f;
        public int attackDamage = 1;
        public LayerMask enemyLayers;

        public float attackRate = 2f;
        private float _nextAttackTime = 0f;
        
        private InputSystem_Actions _inputActions;

        private void Awake()
        {
            _inputActions = new InputSystem_Actions();
            _inputActions.Player.Attack.performed += OnAttack;
        }

        private void OnEnable()
        {
            _inputActions.Enable();
        }

        private void OnDisable()
        {
            _inputActions.Disable();
        }
        
        private void OnAttack(InputAction.CallbackContext obj)
        {
            if (!(Time.time >= _nextAttackTime)) return;
            Attack();
            _nextAttackTime = Time.time + 1f / attackRate;

        }
        
        private void Attack()
        {
            // Detect enemies in range
            var hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayers);

            foreach (var enemy in hitEnemies)
            {
                var damageable = enemy.GetComponentInParent<IDamageable>()
                                 ?? enemy.GetComponentInChildren<IDamageable>();

                damageable?.TakeDamage(attackDamage);
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (attackPoint == null)
                return;

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
    }
}
