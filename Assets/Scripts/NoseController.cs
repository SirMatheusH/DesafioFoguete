using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.Serialization;

public class NoseController : MonoBehaviour
{
    public GameObject parachuteGameObject;
    public GameObject stageRigidBody;
    private Parachute _parachuteScript;
    private float? _maxHeightReached = 0f;

    private FixedJoint _joint;

    [HideInInspector] // Exposto para ser usado pelo script Parachute. 
    public bool isJoined = true;

    private Rigidbody _stageRigidBody;

    private void Start()
    {
        _parachuteScript = parachuteGameObject.GetComponent<Parachute>();
        _joint = gameObject.GetComponent<FixedJoint>();
        _stageRigidBody = stageRigidBody.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        CheckHeight();
        CheckInputs();
    }

    [SuppressMessage("ReSharper", "ConvertIfStatementToSwitchStatement")] // Suprimindo esse aviso também porque um if else fica mais legivel do que um switch case nesse *case*
    private void CheckInputs()
    {
        if (!isJoined && !_parachuteScript.isParachuteOpen && Input.GetKey(KeyCode.LeftControl)) // Se o estagio estiver desconectado e o paraquedas não estiver aberto
        {
            _parachuteScript.OpenParachute();
        }

        // ReSharper disable once InvertIf // Rider quer que eu reduça nesting aqui, mas eu prefiro desse jeito por ficar mais simples de ler
        if (isJoined && Input.GetKey(KeyCode.Space)) // Quando as partes estiverem conectadas, apertar o espaço quebra a junta, separando as partes
        {
            _joint.breakForce = 0; // quebra a junta
            isJoined = false; // seta pra falso pra não permitir que o estagio acelere mais
            _stageRigidBody.drag = 0.1f; // adiciona uma força de arrasto no estágio, por ser menos aerodinamico depois que separado do nariz pontura do foguete
        }
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