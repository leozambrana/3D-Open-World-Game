using UnityEngine;
using UnityEngine.SceneManagement;

namespace Generic
{
    public class MainMenu : MonoBehaviour
    {
        public void StartGame()
        {
            SceneManager.LoadScene("WorldScene"); // Substitui com o nome da sua cena
        }

        public void QuitGame()
        {
            Application.Quit();
            Debug.Log("Saiu do jogo"); // Funciona só no build, no editor não fecha
        }
    }
}
