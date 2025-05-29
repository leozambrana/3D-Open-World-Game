using Interface;
using UnityEngine;

namespace Enemy
{
    public class EnemyAttack : MonoBehaviour
    {
        [Header("Attack Settings")]
        public float attackRange = 2f;
        public int attackDamage = 1;
        public float attackRate = 1.5f;
        private float _nextAttackTime = 0f;

        [Header("Target")]
        public LayerMask playerLayer;

        private Transform _playerTransform;

        private void Start()
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                _playerTransform = playerObj.transform;
        }

        private void Update()
        {
            if (!_playerTransform) return;

            float distanceToPlayer = Vector3.Distance(transform.position, _playerTransform.position);

            if (distanceToPlayer <= attackRange)
            {
                if (Time.time >= _nextAttackTime)
                {
                    print("entrou");
                    Attack();
                    _nextAttackTime = Time.time + 1f / attackRate;
                }
            }
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void Attack()
        {
            Collider[] hitPlayers = Physics.OverlapSphere(transform.position, attackRange, playerLayer);

            foreach (Collider player in hitPlayers)
            {
                IDamageable damageable = player.GetComponentInParent<IDamageable>()
                                         ?? player.GetComponentInChildren<IDamageable>();

                damageable?.TakeDamage(attackDamage);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }
}
