using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace User_Interface.MainMenu
{
    public class SceneController : MonoBehaviour
    {
        public Animator animator;
        public float transitionDelayTime = 1.0f;

        private void Awake()
        {
            animator = GameObject.Find("Transition").GetComponent<Animator>();
        }

        public void LoadLevel()
        {
            StartCoroutine(DelayLoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
        }

        IEnumerator DelayLoadLevel(int index)
        {
            animator.SetTrigger("TriggerTransition");
            yield return new WaitForSeconds(transitionDelayTime);
            SceneManager.LoadScene(index);
        }
    }
}