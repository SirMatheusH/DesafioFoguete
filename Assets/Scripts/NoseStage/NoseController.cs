using Stage;
using UnityEngine;

namespace NoseStage
{
    public class NoseController : MonoBehaviour
    {
        // Game objects
        public GameObject parachuteGameObject;
        public GameObject stageGameObject;
        public GameObject stageSeparation;

        // Rigid bodies
        private Rigidbody _stageRigidBody;
        private Rigidbody _noseRigidBody;

        // Scripts
        private Parachute _parachuteScript;
        private StageSeparationParticles _stageSeparationParticlesScript;
        private StageController _stageController;
        private ParticleController _stageParticleController;
        private ParticleController _particleController;
        
        private AudioController _stageAudioController;
        private AudioController _noseAudioController;

        /**
         * A junta que conecta o nariz ao primeiro estágio
         */
        private FixedJoint _joint;

        /**
         * Altura máxima que o foguete alcançou
         */
        public float maxHeightReached;

        public bool isJoined = true;

        /**
         * Quantidade de combustível com o qual o foguete começa.
         */
        public float initialFuel = 12;

        /**
         * Nível de combustível atual.
         */
        public float currentFuel;

        /**
         * Força da aceleração.
         */
        public float force = 20;

        /**
         * Aceleração máxima.
         */
        public float maxForce = 35;

        private void Start()
        {
            _stageRigidBody = stageGameObject.GetComponent<Rigidbody>();
            _noseRigidBody = gameObject.GetComponent<Rigidbody>();

            _parachuteScript = parachuteGameObject.GetComponent<Parachute>();
            _stageSeparationParticlesScript = stageSeparation.GetComponent<StageSeparationParticles>();
            _stageController = stageGameObject.GetComponent<StageController>();
            _stageParticleController = stageGameObject.transform.GetChild(0).gameObject.GetComponent<ParticleController>();
            _particleController = gameObject.transform.GetChild(1).gameObject.GetComponent<ParticleController>();
            _stageAudioController = stageGameObject.GetComponent<AudioController>();
            _noseAudioController = gameObject.GetComponent<AudioController>();

            _joint = gameObject.GetComponent<FixedJoint>();

            currentFuel = initialFuel;
        }

        private void Update()
        {
            CheckHeight();
            CheckFuelLevels();
        }

        private void FixedUpdate() => CheckInputs(); // CheckInputs interage com a física, so FixedUpdate it is.

        private void CheckInputs()
        {
            if (isJoined && Input.GetKey(KeyCode.Space)) // Quando as partes estiverem conectadas, apertar o espaço quebra a junta, separando as partes
            {
                BreakOff();
            }

            if (!isJoined && !_parachuteScript.isParachuteOpen && Input.GetKey(KeyCode.LeftControl)) // Se o estagio estiver desconectado e o paraquedas não estiver aberto
            {
                _parachuteScript.OpenParachute();
                _particleController.DeleteParticleSystem();
                _noseAudioController.PauseRocketBoosterSfx();
            }

            var controlsEnabled = !isJoined && !_parachuteScript.isParachuteOpen && currentFuel > 0;

            if (!controlsEnabled) return; // pra não checar essa variável em todos os if-elses abaixo

            if (Input.GetKey(KeyCode.LeftShift))
            {
                _noseRigidBody.AddUpwardsForce(gameObject, force);
                currentFuel -= Time.fixedDeltaTime;

                if (force < maxForce) force += 0.1f; // Limite na quantidade de força sendo usada pra levantar o foguete

                // Pra remover a duplicidade de checar duas vezes se Shift está sendo pressionado, isso também arruma um bug no qual ambos efeitos não eram ativados quando o estágio se separava
                // automaticamente.
                if (!_noseAudioController.isPlaying) _noseAudioController.PlayRocketBoosterSfx();
                if (!_particleController.isEmitting) _particleController.StartEmitting();
            }

            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                _particleController.StopEmitting();
                _noseAudioController.PauseRocketBoosterSfx();
            }

            if (Input.GetKey(KeyCode.W)) _noseRigidBody.AddRotationalTorque(Direction.Forwards);
            if (Input.GetKey(KeyCode.A)) _noseRigidBody.AddRotationalTorque(Direction.Left);
            if (Input.GetKey(KeyCode.S)) _noseRigidBody.AddRotationalTorque(Direction.Backwards);
            if (Input.GetKey(KeyCode.D)) _noseRigidBody.AddRotationalTorque(Direction.Right);
        }

        /**
         * Essa função pode ser chamada pelo StageController também caso o estagio use todo o combustivel, e pra evitar a repetição, função :)
         */
        public void BreakOff()
        {
            _joint.breakForce = 0; // quebra a junta
            isJoined = false; // seta pra falso pra não permitir que o estagio acelere mais
            _stageRigidBody.drag = 0.1f; // adiciona uma força de arrasto no estágio, por ser menos aerodinamico depois que separado do nariz pontura do foguete
            _stageController.enabled = false; // desativação do stageController pra não precisar se preocupar com colizões ao reutilizar os mesmos controles
            _stageSeparationParticlesScript.EmitParticles(); // Mostra as partículas de separação dos estágios
            
            // não queria chamar essa função aqui devido o fato do Nariz do foguete não ter nada aver com as particulas do estágio, mas okay
            _stageParticleController.DeleteParticleSystem();

            // Devido ao Nariz ter a função responsável por separar os compartimentos, faz sentido parar o som dos boosters do estágio aqui
            _stageAudioController.PauseRocketBoosterSfx();
        }

        /**
         * A altura é checada dentro do Update devido a alta taxa de atualização, oque aumenta a precisão da medida da altura máxima
         */
        private void CheckHeight()
        {
            if (_noseRigidBody.transform.position.y > maxHeightReached) maxHeightReached = _noseRigidBody.transform.position.y;
        }

        /**
         * Usado para parar de emitir as partículas e de tocar o som do foguete quando o combustível acabar
         */
        private void CheckFuelLevels()
        {
            if (currentFuel <= 0)
            {
                _particleController.StopEmitting();
                _particleController.DeleteParticleSystem();
                _noseAudioController.PauseRocketBoosterSfx();
            }
        }
    }
}