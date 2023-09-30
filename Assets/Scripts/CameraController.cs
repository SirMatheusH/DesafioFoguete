using UnityEngine;

public class Camera : MonoBehaviour
{
    public Transform objectToFollow;
    public bool controlsEnabled;
    public Vector3 initialOffset = Vector3.zero;
    
    private Vector3 _offset;

    private void Start()
    {
        _offset = initialOffset;
    }

    void Update()
    {
        transform.position = objectToFollow.transform.position + _offset; // Faz a camera seguir o objeto especificado continuamente

        if (controlsEnabled && Input.GetKey(KeyCode.UpArrow) && _offset.z < -2) // Permite o zoom in ou out da câmera.
        {
            _offset += new Vector3(0, 0, -5) * -0.005f;
        }
        if (controlsEnabled && Input.GetKey(KeyCode.DownArrow) && _offset.z > -10)
        {
            _offset += new Vector3(0, 0, -5) * 0.005f;
        }
    }
}
