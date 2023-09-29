using System;
using UnityEngine;
using UnityEngine.Serialization;

public class RocketController : MonoBehaviour
{
    public GameObject stage;
    public GameObject parachuteGameObject;
    private Parachute _parachuteScript;
    
    private Rigidbody _noseRigidBody;
    private Rigidbody _stageRigidBody;
    private FixedJoint _joint;
    
    [HideInInspector] // Exposto para ser usado pelo script Parachute. 
    public bool isJoined = true;
    private float? _maxHeightReached = 0f;
    
    /**
     * Maior do que os 5 especificados devido o fato do foguete demorar mais pra sair do chão.
     */
    public float fuel = 20f;
    
    /**
     * Pra criar um efeito mais "realista", o foguete começa acelerando devagar, até atingir um limite, similar aos foguetes na vida real.
     */
    public float force = 10;

    /**
     * Limite máximo da força sendo utilizada pra fazer o foguete voar.
     */
    public float maxForce = 50;
    void Start()
    {       
        _noseRigidBody = gameObject.GetComponent<Rigidbody>();
        _joint = gameObject.GetComponent<FixedJoint>();
        _stageRigidBody = stage.GetComponent<Rigidbody>();
        _parachuteScript = parachuteGameObject.GetComponent<Parachute>();
    }
    
    private void Update()
    {
        CheckHeight();
    }

    private void FixedUpdate()
    {
        CheckInputs(); 
    }
    
    /**
     * Os inputs são checados dentro de FixedUpdate, por ser o local recomendado para fazer interações com a física.
     */
    private void CheckInputs()
    {
        if (isJoined && fuel > 0 && Input.GetKey(KeyCode.LeftShift)) // Se os foguetes estiverem conectados, ainda houver combustivel e shift estiver pressionado
        {
            _noseRigidBody.AddUpwardsForce(gameObject, force); // Adiciona uma força no RigidBody
            
            // Reduz o combustivel somente enquanto o shift estiver sendo segurado (razão no lore: esse foguete não usa combustivel sólidos? https://www.esa.int/Education/Solid_and_liquid_fuel_rockets)
            fuel -= Time.fixedDeltaTime; 
            if (force < maxForce) // Limite na quantidade de força sendo usada pra levantar o foguete
            {
                force += (0.1f);  
            }               
        }
        
        // Permite a rotação do foguete no ar somente enquanto o paraquedas não estiver aberto
        if (!_parachuteScript.isParachuteOpen)
        {
            if (Input.GetKey(KeyCode.W)) _noseRigidBody.AddRotationalTorque(Direction.Forwards);
            if (Input.GetKey(KeyCode.A)) _noseRigidBody.AddRotationalTorque(Direction.Left);
            if (Input.GetKey(KeyCode.S)) _noseRigidBody.AddRotationalTorque(Direction.Backwards);
            if (Input.GetKey(KeyCode.D)) _noseRigidBody.AddRotationalTorque(Direction.Right);
        }
        
        if (isJoined && Input.GetKey(KeyCode.Space)) // Quando as partes estiverem conectadas, apertar o espaço quebra a junta, separando as partes
        {
            _joint.breakForce = 0; // quebra a junta
            isJoined = false; // seta pra falso pra não permitir que o foguete seja acelerado mais
            _stageRigidBody.drag = 0.1f; // adiciona uma força de arrasto no estágio, por ser menos aerodinamico depois que separado do nariz pontura do foguete
        }

        if (!isJoined && !_parachuteScript.isParachuteOpen && Input.GetKeyDown(KeyCode.LeftControl)) // Se os foguetes estiverem desconectados e o paraquedas não estiver aberto
        {
            _parachuteScript.OpenParachute();
        }
    }
    
    /**
     * A altura é checada dentro do Update devido a alta taxa de atualização, oque aumenta a precisão da medida da altura máxima
     */
    private void CheckHeight()
    {

        if ( _noseRigidBody.transform.position.y > _maxHeightReached)
        {
            _maxHeightReached = _noseRigidBody.transform.position.y;
        }
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
                receiver.AddRelativeTorque(new Vector3(0,-1,0) * 5, ForceMode.Acceleration);
                break;
            case Direction.Right:
                receiver.AddRelativeTorque(new Vector3(0,1,0) * 5, ForceMode.Acceleration);
                break;
            case Direction.Forwards:
                receiver.AddRelativeTorque(new Vector3(1,0,0) * 5, ForceMode.Acceleration);
                break;
            case Direction.Backwards:
                receiver.AddRelativeTorque(new Vector3(-1,1,0) * 5, ForceMode.Acceleration);
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
    Right, Left, Forwards, Backwards
}

