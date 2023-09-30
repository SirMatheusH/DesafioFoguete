using System.Collections;
using System.Collections.Generic;
using Stage;
using UnityEngine;

/**
 * Usado pra initializar com os valores corretos todos os elementos relevantes do foguete.
 */
public class Initializer : MonoBehaviour
{

    public GameObject parachuteGameObject;
    private Rigidbody _parachuteRb;
    private MeshCollider _parachuteMc;
    private MeshRenderer _parachuteMr;
    
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
    }
}
