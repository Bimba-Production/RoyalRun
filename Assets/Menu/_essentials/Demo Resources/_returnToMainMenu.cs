using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Scripts
{
    public class _returnToMainMenu : MonoBehaviour
    {
        public void LoadMainMenu()
        {
            SceneManager.LoadScene("Demo");
        }
    }
}
