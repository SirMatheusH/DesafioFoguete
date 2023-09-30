using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Controlador de efeito sonoro genérico, ao anexar esse script e um AudioSource à um GameObject, esse controller pode começar e parar qualquer efeito sonoro.
 *
 * Utilizado no estagio e na parte superior do foguete (nesse caso, preparando pra quando eu implementar um propulsor no nivel difícil do desafio).
 */
public class AudioController : MonoBehaviour
{
    
    private AudioSource _audioSource;
    
    void Start()
    {
        _audioSource = gameObject.GetComponent<AudioSource>(); // O GameObject que esse script está anexado deve ter um componente AudioSource adicionado.
    }

    public void PlayRocketBoosterSfx()
    {
        _audioSource.Play();
    }

    public void PauseRocketBoosterSfx()
    {
        _audioSource.Pause();
    }
}
