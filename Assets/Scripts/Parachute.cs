using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parachute : MonoBehaviour
{

    private MeshRenderer _meshRenderer;
    private MeshCollider _meshCollider;
    private Rigidbody _rigidbody;
    public GameObject rocket;
    private RocketController _rocketController;
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
        _rigidbody = gameObject.GetComponent<Rigidbody>();
        _rocketController = rocket.GetComponent<RocketController>();
        _meshRenderer.enabled = false; // Caso ambos não estajam desativados no editor antes de entrar no GameMode.
        _meshCollider.enabled = false;
        _rigidbody.mass = 0; 
    }

    void Update()
    {
        // Checa se o foguete está separado e se está perto do chão, caso sim, abre o paraquedas
        if (openParachuteAutomatically && !_rocketController.isJoined && _rigidbody.transform.position.y < 100)
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
        _rigidbody.mass = 1;
        _rigidbody.drag = 5;
    }
}
