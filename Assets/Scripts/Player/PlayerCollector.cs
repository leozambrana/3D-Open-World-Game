using UnityEngine;

namespace Player
{
    public class PlayerCollector : MonoBehaviour
    {
        public PlayerUI playerUI; 
        
        public int orbCount = 0;
        private int _orbsSinceLastHeal = 0;
        public int orbsForHeal = 3;
        public int healAmount = 5;


        public void Add(int amount)
        {
            orbCount += amount;
            _orbsSinceLastHeal += amount;

            if (_orbsSinceLastHeal >= orbsForHeal)
            {
                playerUI.Heal(healAmount);
                _orbsSinceLastHeal = 0;
            }
            Debug.Log("Coletou orbe! Total: " + orbCount);
        }
    }
}
