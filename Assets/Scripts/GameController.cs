using Unity.Collections;
using UnityEngine;
using Random = System.Random;

/**
 * Usado pra initializar com os valores corretos todos os elementos relevantes do foguete e controlar a emissão do vento.
 */
public class GameController : MonoBehaviour
{

    public GameObject parachuteGameObject;
    public Rigidbody noseRigidBody;
    public Rigidbody stageRigidBody;
    private Rigidbody _parachuteRb;
    private MeshCollider _parachuteMc;
    private MeshRenderer _parachuteMr;
    
    [ReadOnly]
    public Vector3 windDirection = Vector3.zero;
    
    private void Start()
    {
        
        _parachuteRb = parachuteGameObject.GetComponent<Rigidbody>();
        _parachuteMc = parachuteGameObject.GetComponent<MeshCollider>();
        _parachuteMr = parachuteGameObject.GetComponent<MeshRenderer>();
        
        // Remove a massa e desativa a colisão e a renderização do paraquedas.
        _parachuteRb.mass = 0; 
        _parachuteMc.enabled = false;
        _parachuteMr.enabled = false;
        
        gameObject.transform.position = Vector3.zero; // Coloca o foguete firmemente no chão
        
        RandomWindDirection();
    }

    private void FixedUpdate()
    {
        if (noseRigidBody.transform.position.y < 1000)
        {
            noseRigidBody.AddForce(windDirection);
        }

        if (stageRigidBody.transform.position.y < 1000)
        {
            stageRigidBody.AddForce(windDirection);
        }
    }

    private void RandomWindDirection()
    {
        var random = new Random();

        var x = (float) random.NextDouble();
        // var y = (float) random.NextDouble();
        var z = (float)  random.NextDouble();
        
        windDirection = new Vector3(x, 0f, z) / 2;
    }
}
