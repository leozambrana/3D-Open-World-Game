using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    public class EnemyFollow : MonoBehaviour
    {
        public Animator animator; 
        public Transform player;
        private NavMeshAgent _agent;
        private EnemyAttack _enemyAttack; 

        private void Start()
        {
            _agent = GetComponent<NavMeshAgent>();
            _enemyAttack = GetComponent<EnemyAttack>();
            
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
        }

        private void Update()
        {
            if (!player) return;
            
            var distance = Vector3.Distance(transform.position, player.position);

            if (distance > _enemyAttack.attackRange)
            {
                _agent.isStopped = false;
                _agent.SetDestination(player.position);
                animator.SetFloat("Speed", _agent.velocity.magnitude);
                print("agent velocity" +  _agent.velocity.magnitude);
                animator.ResetTrigger("Attack");
            }
            else
            {
                _agent.isStopped = true;
                animator.SetFloat("Speed", 0);
                // Faz o inimigo olhar para o player (opcional, melhora a sensação de ataque)
                Vector3 lookDirection = (player.position - transform.position).normalized;
                lookDirection.y = 0;
                if (lookDirection != Vector3.zero)
                    transform.rotation = Quaternion.LookRotation(lookDirection);
                animator.SetTrigger("Attack");
            }
        }
    }
}
