using UnityEngine;

/**
  * Minha parte favorita até agora foi aprender a mexer com o ParticleSystem do Unity, 10/10, divertido e fácil de usar.
  * Cores e estilo do efeito dos booster foram escolha ""artistica"", eu poderia ter procurado como fazer um efeito de propulsor de foguete usando o ParticleSystem, mas
  * mas quis criar algo mais "custom" com umas cores mais diferentes.
  */
public class ParticleController : MonoBehaviour
{
    private ParticleSystem _particleSystem;
    private ParticleSystem.EmissionModule _emission;

    public bool isEmitting;
    
    /**
      * Timer nulável pra não precisar usar uma variavel booleana extra
      */
    private float? _deletionTimer = null;

    private void Start()
    {
        _particleSystem = gameObject.GetComponent<ParticleSystem>();
        _emission = _particleSystem.emission;
        StopEmitting(); // Não funciona dentro do Initializer, suspeito que devido ao fato desse MonoBehavior não ter sido instanciado ainda naquele ponto da execução.
        isEmitting = false;
    }

    private void Update()
    {
        // Timer para deletar o GameObject após não ser mais necessário
        if (_deletionTimer is > 0) // Mesmo caso do StageSeparationParticles, só é verdadeiro quando o timer não for mais nulo
        {
            _deletionTimer -= Time.deltaTime;
        } else if (_deletionTimer <= 0) Destroy(gameObject);
        
    }

    public void StartEmitting()
    {
        _emission.enabled = true;
        isEmitting = true;
    }

    public void StopEmitting()
    {
        _emission.enabled = false;
        isEmitting = false;
    }

    public void DeleteParticleSystem()
    { 
        StopEmitting();
        // O timer é inicializado com o valor da lifetime das particulas, pra assegurar que elas tenham terminado de "existir" antes de deletar o objeto
        _deletionTimer = _particleSystem.main.startLifetime.constant;
        isEmitting = false;
    }
}