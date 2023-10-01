using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace User_Interface.MainMenu
{
    public class SceneController : MonoBehaviour
    {
        public Animator animator;

        private void Awake()
        {
            animator = GameObject.Find("Transition").GetComponent<Animator>();
        }

        /**
         * Abre a cena do foguete usando uma animação de transição
         */
        public void LoadLevel()
        {
            StartCoroutine(DelayLoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
        }

        /**
         * Uma coroutine é utilizada pra atrasar o carregamento da nova cena pra deixar a transição terminar primeiro.   
         */
        private IEnumerator DelayLoadLevel(int index)
        {
            animator.SetTrigger("TriggerTransition");
            yield return new WaitForSeconds(1.0f);
            SceneManager.LoadScene(index);
        }
    }
}