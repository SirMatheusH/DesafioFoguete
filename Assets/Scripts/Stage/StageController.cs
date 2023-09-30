using System;
using NoseStage;
using UnityEngine;

namespace Stage
{
    public class StageController : MonoBehaviour
    {
        public GameObject noseGameObject;

        private Rigidbody _stageRigidBody;

        private AudioController _audioController;
        
        private NoseController _noseController;
        public StageParticleController stageParticleController;

        /**
         * Maior do que os 5 especificados devido o fato do foguete demorar mais pra sair do chão.
         */
        public float fuel = 25f;

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
        }

        private void Update() => CheckFuelLevels();

        private void FixedUpdate() => CheckInputs();

        /**
         * Os inputs são checados dentro de FixedUpdate pois é onde eu tô aplicando forças no RigidBody e é o local certo para fazer interações com a física.
         */
        private void CheckInputs()
        {
            var canAccelerate = _noseController.isJoined && fuel > 0;
            
            if (canAccelerate && Input.GetKey(KeyCode.LeftShift))
            {
                _stageRigidBody.AddUpwardsForce(gameObject, force); // Adiciona uma força no RigidBody

                // Reduz o combustivel somente enquanto o shift estiver sendo segurado
                // (razão no lore: esse foguete usa combustivel líquido? https://www.esa.int/Education/Solid_and_liquid_fuel_rockets)
                fuel -= Time.fixedDeltaTime;

                if (force < maxForce) force += (0.1f); // Limite na quantidade de força sendo usada pra levantar o foguete
                
            }

            if (canAccelerate && Input.GetKeyDown(KeyCode.LeftShift))
            {
                stageParticleController.StartEmitting();
                _audioController.PlayRocketBoosterSfx();
            } else if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                stageParticleController.StopEmitting();
                _audioController.PauseRocketBoosterSfx();
            }
            

            // Permite a rotação do foguete no ar somente enquanto o estagio estiver conectado com o foguete
            if (_noseController.isJoined)
            {
                if (Input.GetKey(KeyCode.W)) _stageRigidBody.AddRotationalTorque(Direction.Forwards);
                if (Input.GetKey(KeyCode.A)) _stageRigidBody.AddRotationalTorque(Direction.Left);
                if (Input.GetKey(KeyCode.S)) _stageRigidBody.AddRotationalTorque(Direction.Backwards);
                if (Input.GetKey(KeyCode.D)) _stageRigidBody.AddRotationalTorque(Direction.Right);
            }
        }
        
        private void CheckFuelLevels() // Checa o nivel de combustivel e desconecta automaticamente do resto do foguete quando o combustivel acaba.
        {
            if (fuel <= 0)
            {
                _noseController.BreakOff();
                stageParticleController.StopEmitting(); // :)
            }
        }

        private void OnDisable()
        {
            stageParticleController.StopEmitting(); // Pra se certificar mesmo que aquelas particulas não vão ficar saindo;
        }
    }

    /**
     * Funções criadas para simplificar, abstrair, e diminuir repetições de código.
     */
    public static class Extensions
    {
        public static void AddUpwardsForce(this Rigidbody receiver, GameObject gameObject, float force)
        {
            receiver.AddForce(gameObject.transform.forward * force, ForceMode.Force); // transform.forward e não transform.up devido à orientação do nariz do foguete.
        }

        public static void AddRotationalTorque(this Rigidbody receiver, Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                    receiver.AddRelativeTorque(new Vector3(0, -1, 0) * 5, ForceMode.Acceleration);
                    break;
                case Direction.Right:
                    receiver.AddRelativeTorque(new Vector3(0, 1, 0) * 5, ForceMode.Acceleration);
                    break;
                case Direction.Forwards:
                    receiver.AddRelativeTorque(new Vector3(1, 0, 0) * 5, ForceMode.Acceleration);
                    break;
                case Direction.Backwards:
                    receiver.AddRelativeTorque(new Vector3(-1, 1, 0) * 5, ForceMode.Acceleration);
                    break;
                default:
                    return;
            }
        }
    }

    /**
     * As direções são relativas à orientação fixa da câmera
     */
    public enum Direction
    {
        Right,
        Left,
        Forwards,
        Backwards
    }
}