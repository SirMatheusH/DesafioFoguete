using UnityEngine;

public class StageController : MonoBehaviour
{
    public GameObject noseGameObject;

    private NoseController _noseController;
    private Rigidbody _stageRigidBody;
    
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
    
    private void Start()
    {       
        _stageRigidBody = gameObject.GetComponent<Rigidbody>();
        _noseController = noseGameObject.GetComponent<NoseController>();
    }

    private void FixedUpdate()
    {
        CheckInputs(); 
    }
    
    /**
     * Os inputs são checados dentro de FixedUpdate pois é onde eu tô aplicando forças no RigidBody e é o local certo para fazer interações com a física.
     */
    private void CheckInputs()
    {
        if (_noseController.isJoined && fuel > 0 && Input.GetKey(KeyCode.LeftShift)) // Se os foguetes estiverem conectados, ainda houver combustivel e shift estiver pressionado
        {
            _stageRigidBody.AddUpwardsForce(gameObject, force); // Adiciona uma força no RigidBody
            
            // Reduz o combustivel somente enquanto o shift estiver sendo segurado (razão no lore: esse foguete não usa combustivel sólidos? https://www.esa.int/Education/Solid_and_liquid_fuel_rockets)
            fuel -= Time.fixedDeltaTime; 
            if (force < maxForce) // Limite na quantidade de força sendo usada pra levantar o foguete
            {
                force += (0.1f);  
            }               
        }
        
        // Permite a rotação do foguete no ar somente enquanto o paraquedas não estiver aberto
        if (_noseController.isJoined)
        {
            if (Input.GetKey(KeyCode.W)) _stageRigidBody.AddRotationalTorque(Direction.Forwards);
            if (Input.GetKey(KeyCode.A)) _stageRigidBody.AddRotationalTorque(Direction.Left);
            if (Input.GetKey(KeyCode.S)) _stageRigidBody.AddRotationalTorque(Direction.Backwards);
            if (Input.GetKey(KeyCode.D)) _stageRigidBody.AddRotationalTorque(Direction.Right);
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

