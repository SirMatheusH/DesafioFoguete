using NoseStage;
using Stage;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Stage.Miscellaneous;


namespace User_Interface.RocketScene
{
    public class InterfaceController : MonoBehaviour
    {

        public TextMeshProUGUI speedometer;
        public TextMeshProUGUI maxHeight;
        public TextMeshProUGUI currentHeight;
        public TextMeshProUGUI fuelText;

        // Objetos e scripts necessários pra mostrar informações na interface 
        public GameObject noseGameObject;
        private NoseController _noseController;
        private Rigidbody _noseRigidbody;
        
        public GameObject stageGameObject;
        private StageController _stageController;

        private void Start()
        {
            _noseController = noseGameObject.GetComponent<NoseController>();
            _noseRigidbody = noseGameObject.GetComponent<Rigidbody>();
            _stageController = stageGameObject.GetComponent<StageController>();
        }


        void Update()
        {
            CheckInputs();
            UpdateUi();
        }

        private void UpdateUi()
        {
            speedometer.text = ((int)_noseRigidbody.velocity.magnitude) + "m/s";
            maxHeight.text = "Altura máxima: " +  (int)_noseController.maxHeightReached + "m";
            currentHeight.text = "Altura atual: " + (int)_noseRigidbody.transform.position.y + "m";

            if (_noseController.isJoined)
            {
                var percentage = PercentageBetweenTwoValues(_stageController.currentFuel, _stageController.initialFuel);
                fuelText.text = "Combustível: " + percentage + "%";
                print("perc " + percentage);
            }
                
            else fuelText.text = "Combustível: " + PercentageBetweenTwoValues(_noseController.currentFuel, _noseController.initialFuel) + "%";
        }

        /**
         * Essa função devia ter seu próprio script dentro do objeto SceneManager, mas vou deixar aqui mesmo
         */
        private void CheckInputs()
        {
            if (Input.GetKeyDown(KeyCode.R)) ReloadScene();
        }

        /**
         * Simplesmente reinicia a cena.
         */
        private void ReloadScene()
        {
            SceneManager.LoadScene(1);
        }
        
    }
}