using TMPro;
using UnityEngine;

namespace UI
{
    public class WaveUI : MonoBehaviour
    {
        public TextMeshProUGUI waveText;

        public void ShowWave(int waveNumber)
        {
            CancelInvoke();
            
            if (waveNumber % 5 == 0)
                waveText.text = "BOSS WAVE!";
            else
                waveText.text = "Wave " + waveNumber;
            waveText.gameObject.SetActive(true);
            Invoke(nameof(Hide), 2f); // Some depois de 2 segundos
        }

        private void Hide()
        {
            waveText.gameObject.SetActive(false);
        }
    }
}
