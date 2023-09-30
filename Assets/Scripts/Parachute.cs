using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Parachute : MonoBehaviour
{

    private MeshRenderer _meshRenderer;
    private MeshCollider _meshCollider;
    private Rigidbody _parachuteRigidbody;
    public GameObject nose;
    private NoseController _noseController;
    public bool openParachuteAutomatically = true;

    /**
     * Usada pra não chamar OpenParachute multiplas vezes
     */
    [HideInInspector] // Desnecessário mostrar no Inspector
    public bool isParachuteOpen;
    
    void Start()
    {
        _meshRenderer = gameObject.GetComponent<MeshRenderer>();
        _meshCollider = gameObject.GetComponent<MeshCollider>();
        _parachuteRigidbody = gameObject.GetComponent<Rigidbody>();
        _noseController = nose.GetComponent<NoseController>();
        _meshRenderer.enabled = false; // Caso ambos não estajam desativados no editor antes de entrar no GameMode.
        _meshCollider.enabled = false;
        _parachuteRigidbody.mass = 0; 
    }

    void Update()
    {
        // Checa se o foguete está separado e se está perto do chão, caso sim, abre o paraquedas
        if (openParachuteAutomatically && !_noseController.isJoined && _parachuteRigidbody.transform.position.y < 100)
        {
            OpenParachute();
        }
    }
    
    /**
     * Abre o paraquedas, ativando o collider e o mesh do paraquedas, aumenta a massa pra 1 (o paraquedas sempre começa sem massa pra não afetar o voô do resto do foguete),
     * assim como o drag, fazendo com que o foguete desça devagar (e a diferença entre a massa do paraquedas e do nariz do foguete faz com que o foguete aponte pra baixo
     * durante a descida)
     */
    public void OpenParachute()
    {
        isParachuteOpen = true;
        _meshRenderer.enabled = true;
        _meshCollider.enabled = true;
        _parachuteRigidbody.mass = 1;
        _parachuteRigidbody.drag = 5;
    }
}