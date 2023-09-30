using System.Diagnostics.CodeAnalysis;
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

        // Scripts
        private Parachute _parachuteScript;
        private StageSeparationParticles _stageSeparationParticlesScript;
        private StageController _stageController;
        private StageParticleController _stageParticleController;
        
        // Other variables
        /**
          * A junta que conecta o nariz ao primeiro estágio
          */
        private FixedJoint _joint;
        
        private float _maxHeightReached;

        [HideInInspector] // Exposto para os outros scripts. 
        public bool isJoined = true;

        private void Start()
        {
            _stageRigidBody = stageGameObject.GetComponent<Rigidbody>();
            _parachuteScript = parachuteGameObject.GetComponent<Parachute>();
            _stageSeparationParticlesScript = stageSeparation.GetComponent<StageSeparationParticles>();
            _stageController = stageGameObject.GetComponent<StageController>();
            _stageParticleController = stageGameObject.transform.GetChild(0).gameObject.GetComponent<StageParticleController>();
            _joint = gameObject.GetComponent<FixedJoint>();
        }

        private void Update()
        {
            CheckHeight();
            CheckInputs(); // Diferente do CheckInputs do StageController, esse aqui não interage com a física, então usar ele no Update() é suave.
        }

        [SuppressMessage("ReSharper", "ConvertIfStatementToSwitchStatement")] // Suprimindo esse aviso também porque um if else fica mais legivel do que um switch case nesse *case*
        private void CheckInputs()
        {
            if (isJoined && Input.GetKey(KeyCode.Space)) // Quando as partes estiverem conectadas, apertar o espaço quebra a junta, separando as partes
            {
                BreakOff();
            }
            
            if (!isJoined && !_parachuteScript.isParachuteOpen && Input.GetKey(KeyCode.LeftControl)) // Se o estagio estiver desconectado e o paraquedas não estiver aberto
            {
                _parachuteScript.OpenParachute();
            }
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
            _stageSeparationParticlesScript.EmitParticles();
            // não queria chamar essa função aqui devido o fato do Nariz do foguete não ter nada aver com as particulas do estágio, mas okay
            _stageParticleController.DeleteParticleSystem(); 
        }
        
        /**
         * A altura é checada dentro do Update devido a alta taxa de atualização, oque aumenta a precisão da medida da altura máxima
         */
        private void CheckHeight()
        {
            if ( _stageRigidBody.transform.position.y > _maxHeightReached)
            {
                _maxHeightReached = _stageRigidBody.transform.position.y;
            }
        }
    }
}