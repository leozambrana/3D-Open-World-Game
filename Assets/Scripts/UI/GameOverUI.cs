using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class GameOverUI : MonoBehaviour
    {
        public GameObject gameOverPanel;
        public TextMeshProUGUI waveText;
        public TextMeshProUGUI orbText;
        public Button restartButton;

        private void Start()
        {
            gameOverPanel.SetActive(false);
            restartButton.onClick.AddListener(RestartGame);
        }

        public void Show(int wave, int orbs)
        {
            waveText.text = "Wave: " + wave;
            orbText.text = "Orbs: " + orbs;
            gameOverPanel.SetActive(true);

            Time.timeScale = 0f; // Pausa o jogo
            
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public void RestartGame()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            
            Time.timeScale = 1f; // Volta o tempo
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
