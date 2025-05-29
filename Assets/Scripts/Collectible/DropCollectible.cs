using Player;
using UnityEngine;

namespace Collectible
{
    public class DropCollectable : MonoBehaviour
    {
        public int value = 1;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                // Adiciona ao contador do player
                PlayerCollector collector = other.GetComponent<PlayerCollector >();
                
                
                if (collector != null)
                {
                    collector.Add(value);
                }

                // Efeito, som, etc. (opcional)

                Destroy(gameObject);
            }
        }
    }
}
