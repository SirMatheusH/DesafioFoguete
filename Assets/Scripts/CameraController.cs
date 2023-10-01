using UnityEngine;

/**
 * Pra câmera decidi fazer um script genérico
 */
public class Camera : MonoBehaviour
{
    /**
     * Objeto cujo qual a câmera irá seguir
     */
    public Transform objectToFollow;
    
    /**
     * Permite ou não o controle da câmera
     */
    public bool controlsEnabled;
    
    /**
     * Offset inicial da camera com relação ao foguete.
     */
    public Vector3 initialOffset = Vector3.zero;
    private Vector3 _offset;
    
    /**
     * Distancia máxima entre a camera e o objeto sendo seguido.
     */
    public float zoomOutLimit = 15;

    private void Start()
    {
        _offset = initialOffset;
    }

    void Update()
    {
        transform.position = objectToFollow.transform.position + _offset; // Faz a camera seguir o objeto especificado continuamente
        // Permite o zoom in ou out da câmera.

        if (!controlsEnabled) return;
        
        if (Input.GetKey(KeyCode.UpArrow) && _offset.z < -2)
        {
            _offset += new Vector3(0, 0, -5) * -0.005f;
        }
        if (Input.GetKey(KeyCode.DownArrow) && _offset.z > (-zoomOutLimit)) // negativado devido a posição da camera com relacao ao objeto
        {
            _offset += new Vector3(0, 0, -5) * 0.005f;
        }
    }
}
