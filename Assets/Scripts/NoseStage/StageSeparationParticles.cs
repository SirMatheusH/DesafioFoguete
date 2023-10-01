using UnityEngine;

namespace NoseStage
{
    public class StageSeparationParticles : MonoBehaviour
    {
        private ParticleSystem _particleSystem;
        private ParticleSystem.EmissionModule _emissionModule;

        /**
         * Timer nulável pra não precisar usar uma variavel booleana extra
         */
        private float? _timerBeforeDeletion = null;
        
        private void Start()
        {
            _particleSystem = gameObject.GetComponent<ParticleSystem>();
            _emissionModule = _particleSystem.emission;
            _emissionModule.enabled = false;
        }

        private void Update()
        {
            // Checa se o timer não é mais nulo, se não for, um timer começa, e no final o GameObject é deletado
            if (_timerBeforeDeletion is > 0) // Só é verdadeiro quando _timerBeforeDeletion não for mais nulo
            {
                _timerBeforeDeletion -= Time.deltaTime;
            }
            else if (_timerBeforeDeletion <= 0) Destroy(gameObject);
        }

        /**
         * As particulas de separação do estágio só vão ser emitidas uma vez, então um timer é usado pra deletar o GameObject depois de todas as particulas serem emitidas.
         */
        public void EmitParticles()
        {
            _emissionModule.enabled = true;
            _particleSystem.Emit(new ParticleSystem.EmitParams(), 1000);
            // O timer é inicializado com o valor da lifetime das particulas, pra assegurar que elas tenham terminado de "existir" antes de deletar o objeto
            _timerBeforeDeletion = _particleSystem.main.startLifetime.constant;
        }
    }
}