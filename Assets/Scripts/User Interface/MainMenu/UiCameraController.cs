using UnityEngine;
using Random = System.Random;

namespace User_Interface.MainMenu
{
    /**
      * Controla a câmera usada na interface da cena "MenuScene"
      */
    public class UiCameraController : MonoBehaviour
    {
        /**
         * Direção de rotação da câmera no menu principal
         */
        public Vector3 rotationVector = Vector3.zero;

        private void Start()
        {
            GenerateRandomRotationVector();
        }

        private void GenerateRandomRotationVector()
        {
            var random = new Random();
            var x = (float)random.NextDouble();
            var y = (float)random.NextDouble();
            var z = (float)random.NextDouble();
            rotationVector = new Vector3(x, y, z) / 3; // Dividido pra 3 pra diminuir a velocidade de rotação.
        }

        private void Update()
        {
            // O Skybox é tão bonito, seria uma pena só ser capaz de ver uma parte fixa dele
            // (o asset do skybox também está usando um espaço imenso, então vou usar ele mais um pouquinho pra justificar os 300MB no disco)
            gameObject.transform.Rotate(rotationVector);

            // Muda a direção de rotação da câmera
            if (Input.GetKeyDown(KeyCode.R)) GenerateRandomRotationVector();
        }
    }
}