using NoseStage;
using UnityEngine;

namespace Stage
{
    public class StageController : MonoBehaviour
    {
        // Objetos e scripts
        public GameObject noseGameObject;
        private Rigidbody _stageRigidBody;
        private AudioController _audioController;
        private NoseController _noseController;
        public ParticleController particleController;

        /**
         * Nível de combustível com o qual o estágio começa.
         */
        public float initialFuel = 25f;

        /**
         * Nível de combustível atual.
         */
        public float currentFuel;

        /**
         * Pra criar um efeito mais "realista", o foguete começa acelerando devagar, até atingir um limite.
         */
        public float force = 25;

        /**
         * Limite máximo da força sendo utilizada pra fazer o foguete voar.
         */
        public float maxForce = 60;

        private void Start()
        {
            _stageRigidBody = gameObject.GetComponent<Rigidbody>();
            _audioController = gameObject.GetComponent<AudioController>();
            _noseController = noseGameObject.GetComponent<NoseController>();
            currentFuel = initialFuel;
        }

        private void Update() => CheckFuelLevels();

        private void FixedUpdate() => CheckInputs();

        /**
         * Os inputs são checados dentro de FixedUpdate pois é onde eu tô aplicando forças no RigidBody e é o local certo para fazer interações com a física.
         */
        private void CheckInputs()
        {
            var controlsEnabled = _noseController.isJoined && currentFuel > 0;

            if (!controlsEnabled) return;

            if (Input.GetKey(KeyCode.LeftShift))
            {
                _stageRigidBody.AddUpwardsForce(gameObject, force); // Adiciona uma força no RigidBody

                // Reduz o combustivel somente enquanto o shift estiver sendo segurado
                // (razão no lore: esse foguete usa combustivel líquido? https://www.esa.int/Education/Solid_and_liquid_fuel_rockets)
                currentFuel -= Time.fixedDeltaTime;

                if (force < maxForce) force += (0.1f); // Limite na quantidade de força sendo usada pra levantar o foguete
            }

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                particleController.StartEmitting();
                _audioController.PlayRocketBoosterSfx();
            }
            else if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                particleController.StopEmitting();
                _audioController.PauseRocketBoosterSfx();
            }
            
            // Permite a rotação do foguete no ar somente enquanto o estagio estiver conectado com o foguete
            if (Input.GetKey(KeyCode.W)) _stageRigidBody.AddRotationalTorque(Direction.Forwards);
            if (Input.GetKey(KeyCode.A)) _stageRigidBody.AddRotationalTorque(Direction.Left);
            if (Input.GetKey(KeyCode.S)) _stageRigidBody.AddRotationalTorque(Direction.Backwards);
            if (Input.GetKey(KeyCode.D)) _stageRigidBody.AddRotationalTorque(Direction.Right);
        }

        /**
         * Checa o nivel de combustivel e desconecta automaticamente do resto do foguete quando o combustivel acaba.
         */
        private void CheckFuelLevels()
        {
            if (currentFuel <= 0)
            {
                _noseController.BreakOff();
                particleController.StopEmitting(); // :)
            }
        }
    }
}