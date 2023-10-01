using UnityEngine;
using UnityEngine.SceneManagement;

namespace User_Interface.MainMenu
{
    public class Interface : MonoBehaviour
    {
        public void OpenRocketScene()
        {
            SceneManager.LoadScene(1, LoadSceneMode.Single);
        }
    }
}
